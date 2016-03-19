using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Helpers.Utils
{
    public static class AgeCalulator
    {
        public static int CalcAge(DateTime bday)
        {

            var today = DateTime.Today;
            int age = today.Year - bday.Year;
            if (bday > today.AddYears(-age)) age--;
            return age;
        }
    }
}
