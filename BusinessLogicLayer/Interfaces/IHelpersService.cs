using System.Text;

namespace BusinessLogicLayer.Interfaces
{
    public interface IHelpersService
    {
        public StringBuilder EmailBodyPassword_New(string Username, string TokenURL);

        public StringBuilder EmailBodyPassword_Change(string Username);

        public StringBuilder EmailBodyAccount_Welcome(string Username, string Name);
    }
}
