using Cesium.ViewModels.System;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Core.Validation
{
    public class UserModelValidator: AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            //RuleFor(x => x.Id)
            //   .NotEmpty()
            //   .WithMessage("用户Id不能为空");
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("用户名称不能为空");
        }

  
    }
}
