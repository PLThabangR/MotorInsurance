using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

//record to represent the result of an access token generation operation,
//containing the access token string and its expiration time in UTC.
public record AccessTokenResult(string AccessToken,DateTime ExpiresAtUtc);

public interface ITokenService
{
    //Method to generate an access token for a given user ID and roles.
    //Returns an AccessTokenResult containing the generated access token and its expiration time.
    AccessTokenResult GenerateAccessToken(string userId,string email, IEnumerable<string> roles);
    string CreateRefreshTokenPlainText();
    string HashRefreshToken(string plainText);

}
