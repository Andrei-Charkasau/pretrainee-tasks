namespace InnoShop.Auth.Services.DtoModels
{
    public class ResetPasswordDto
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
