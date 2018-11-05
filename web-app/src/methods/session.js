import { sessionService } from "redux-react-session";

export const getSession = () => {
  return sessionService.loadSession();
};

export const getUser = () => {
  return sessionService.loadUser();
};

export const deleteSession = () => {
  return sessionService.deleteSession();
};

export const deleteUser = () => {
  return sessionService.deleteUser();
};

export const saveSession = session => {
  return sessionService.saveSession(session);
};

export const saveUser = user => {
  return sessionService.saveUser(user);
};
