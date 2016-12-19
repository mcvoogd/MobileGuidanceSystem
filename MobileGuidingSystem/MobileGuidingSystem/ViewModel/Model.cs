using MobileGuidingSystem.Model;

namespace MobileGuidingSystem.ViewModel
{
    public abstract class Model
    {
        protected User user;

        public abstract void NextPage(object user); 
    }
}