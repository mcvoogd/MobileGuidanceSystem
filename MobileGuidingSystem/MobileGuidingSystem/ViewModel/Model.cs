using System;
using MobileGuidingSystem.Data;

namespace MobileGuidingSystem.ViewModel
{
    public abstract class Model
    {
        User user;
        public abstract void NextPage(object user); 
    }
}