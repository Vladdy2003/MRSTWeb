using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvenimentMD.Domain.Model;

namespace EvenimentMD.BusinessLogic.Interface
{
    public interface ISignUp
    {
        string SignUpLogic(UserSignUpData data);
        bool EmailExists(string email);
        bool PhoneNumberExists(string phoneNumber);
    }
}