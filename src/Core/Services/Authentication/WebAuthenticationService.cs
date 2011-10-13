using CommunitySite.Core.Data;
using CommunitySite.Core.Domain;

namespace CommunitySite.Core.Services.Authentication
{
    public class WebAuthenticationService:AuthenticationService
    {
        readonly MemberRepository _memberRepository;

        public WebAuthenticationService(MemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public bool Authenticate(string username, string password)
        {

           Member member=  _memberRepository.GetByUsername(username);
            return member != null &&  member.Password == password;
        }

        public void SignIn(string validUsername)
        {
            throw new System.NotImplementedException();
        }
    }
}
