namespace OnlineEducation.Dtos.AccountDtos
{
    public class ValidateResultDto
    {
        public Boolean IsValid;
        public List<String> ErrorMessage;

        public ValidateResultDto()
        {
            this.IsValid = true;
            this.ErrorMessage = new List<String>();
        }
    }
}
