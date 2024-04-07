using DP_backend.Models.DTOs.TSUAccounts;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;

namespace DP_backend.Services
{
    public interface ITSUAccountService
    {

        Task<TSUAuthResponseDTO> GetAuthData(string token);

        Task<TSUAccountsUserModelDTO> GetUserModelByAccountId(Guid accountId);

        bool IsValidTsuAccountEmail(string email);

        string GetAuthLink();
    }
    public class TSUAccountService: ITSUAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClientWithAuthorization;
        private readonly string _privateAuthEndpoint;
        private readonly string _publicAuthEndpoint;
        private readonly string _tsuApplicationId;
        private readonly string _secretToken;
        private readonly string _getUserModelByIdRequest;

        public TSUAccountService(IConfiguration configuration)
        {
            var tsuAccountsSection = configuration.GetSection("TSUAccounts");
            _privateAuthEndpoint = tsuAccountsSection["PrivateAuthEndpoint"];
            _publicAuthEndpoint = tsuAccountsSection["PublicAuthEndpoint"];
            _tsuApplicationId = tsuAccountsSection["AppID"];
            _secretToken = tsuAccountsSection["SecretToken"];
            _getUserModelByIdRequest = tsuAccountsSection["GetUserModelByIdRequest"];

            _httpClient = new HttpClient();
            _httpClientWithAuthorization = new HttpClient
            {
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Encoding.ASCII.GetBytes(
                            $"{tsuAccountsSection["BasicAuthenticationUsername"]}:{tsuAccountsSection["BasicAuthenticationPassword"]}")))
                }
            };

        }

        public string GetAuthLink()
        {
            return _publicAuthEndpoint + _tsuApplicationId;
        }
        public async Task<TSUAuthResponseDTO> GetAuthData(string token)
        {
            try
            {
                var requestBody = new TSUAuthRequestDTO
                {
                    Token = token,
                    ApplicationId = _tsuApplicationId,
                    SecreteKey = _secretToken
                };
                var response = await _httpClient.PostAsync(_privateAuthEndpoint,
                    new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                var responseMsg = await response.Content.ReadAsStringAsync();
                try
                {
                    var result = JsonConvert.DeserializeObject<TSUAuthResponseDTO>(responseMsg);
                    return result;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<TSUAccountsUserModelDTO> GetUserModelByAccountId(Guid accountId)
        {
            var response = await _httpClientWithAuthorization.GetAsync(_getUserModelByIdRequest + accountId);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ArgumentException("Invalid accountId");
            }

            if (response.IsSuccessStatusCode)
            {
                var responseMsg = await response.Content.ReadAsStringAsync();

                try
                {
                    var result = JsonConvert.DeserializeObject<TSUAccountsUserModelDTO>(responseMsg);
                    char[] charsToTrim = { ' ', '\r', '\n' };
                    result.Email = result.Email.Trim(charsToTrim);
                    return result;
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            if (response.StatusCode == (HttpStatusCode)498)
            {
                return null;
            }
            throw new HttpRequestException("Errors appears during sending request to API TSU Accounts");
        }


        public bool IsValidTsuAccountEmail(string email)
        {
            try
            {
                var address = new MailAddress(email);
            }
            catch
            {
                return false;
            }

            return true;

        }

    }
}
