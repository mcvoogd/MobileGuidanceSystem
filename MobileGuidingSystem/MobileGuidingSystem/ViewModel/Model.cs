using MobileGuidingSystem.Model;

namespace MobileGuidingSystem.ViewModel
{
    public abstract class Model
    {
        protected User User;

        public abstract void NextPage(object user); 
    }
}