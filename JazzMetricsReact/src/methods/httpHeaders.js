import base64 from "base-64";

import authClient from "./auth";
import Config from "../global/config";
import { handleError } from "./errorHandler";

export const getBasicHeaders = () => {
  const headers = new Headers();
  addHeaders(headers);
  headers.append(
    "Authorization",
    "Basic " +
      base64.encode(
        base64.decode(Config.TXT) + ":" + base64.decode(Config.SETTING)
      )
  );

  return headers;
};

export const getBearerHeaders = async (token = undefined) => {
  if (token === undefined) {
    try {
      token = await authClient.getToken();
    } catch (err) {
      await handleError(err, "httpHeaders", "getBearerHeaders");
    }
  }

  const headers = new Headers();
  addHeaders(headers);
  headers.append("Authorization", "Bearer " + token);

  return headers;
};

const addHeaders = headers => {
  headers.append("Accept", "application/json");
  headers.append("Content-Type", "application/json");
};
