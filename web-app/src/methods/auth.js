import jwt_decode from "jwt-decode";

import { fetchBasicPost, fetchBearerPost } from "./fetchWithTimeout";
import {
  saveSession,
  saveUser,
  getSession,
  getUser,
  deleteUser,
  deleteSession
} from "./session";

class Auth {
  getUser = async () => {
    return await getUser();
  };

  getToken = async () => {
    return await getSession();
  };

  silentAuth = async () => {
    if (await getSession()) {
      const response = await fetchBearerPost("token");
      if (response.status === 200) {
        const data = await response.json();
        this.setTime(data.token);
        await saveSession(data.token);
      } else {
        this.signOut();
      }
    }
  };

  setTime = token => {
    sessionStorage.setItem("expiresAt", jwt_decode(token).exp);
  };

  isAuthenticated = () => {
    return Date.now().valueOf() / 1000 < sessionStorage.getItem("expiresAt"); //UTC time!
  };

  signIn = async userValues => {
    const response = await fetchBasicPost("user", userValues);
    if (response.status >= 200 && response.status <= 304) {
      const data = await response.json();
      if (!data || data.length === 0) {
        return "server nenalezen";
      } else {
        if (data.properUser) {
          this.setTime(data.token);
          await Promise.all([saveUser(data.user), saveSession(data.token)]);
        } else {
          return data.message;
        }
      }
    } else {
      return "při zpracování požadavku nastala chyba";
    }
    return undefined;
  };

  signOut = async () => {
    sessionStorage.removeItem("expiresAt");
    await Promise.all([deleteUser(), deleteSession()]);
  };
}

const authClient = new Auth();

export default authClient;
