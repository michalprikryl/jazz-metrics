import { fetchBasicPost } from "./fetchWithTimeout";
import authClient from "./auth";
import Package from "../../package.json";

export const handleError = async (error, module, func) => {
  if (error.message !== "Timeout has passed") {
    await reportError({
      name: error.name || error,
      message: error.message,
      module,
      function: func
    });
    return true;
  } else {
    return false;
  }
};

export const reportError = async err => {
  let userString = "undefined user";
  try {
    const user = await authClient.getUser();
    userString = `${user.firstName} ${user.lastName} > ${user.email}`;
  } catch (error) {}

  try {
    await fetchBasicPost("error", {
      message: err.message,
      user: `JM - ${userString}`,
      exceptionType: err.name,
      module: err.module,
      function: err.function,
      exceptionMessage: JSON.stringify(err),
      innerExceptionMessage: `Version - ${Package.version}`
    });
  } catch (error) {}
};
