namespace ServiceAbstraction
{
    public interface IEmailValidatorService
    {
        bool Validate(string email);
    }
}
