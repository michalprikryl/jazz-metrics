import Config from "../global/config";
import { getBearerHeaders, getBasicHeaders } from "./httpHeaders";

export async function fetchBearerPut(endpoint, data, timeout = 30000) {
  return await fetchBase(endpoint, data, timeout, "PUT");
}

export async function fetchBearerPost(endpoint, data, timeout = 30000) {
  return await fetchBase(endpoint, data, timeout, "POST");
}

export async function fetchBasicPost(endpoint, data, timeout = 30000) {
  return await fetchBase(endpoint, data, timeout, "POST", true);
}

export async function fetchBearerGet(endpoint, timeout = 30000) {
  return await fetchBase(endpoint, undefined, timeout, "GET");
}

export async function fetchBasicGet(endpoint, timeout = 30000) {
  return await fetchBase(endpoint, undefined, timeout, "GET", true);
}

async function fetchBase(endpoint, data, timeout, method, basic) {
  return await fetchWithTimeout(
    `${Config.BASE_URL}api/${endpoint}`,
    {
      method,
      headers: basic ? getBasicHeaders() : await getBearerHeaders(),
      body: data ? JSON.stringify(data) : data
    },
    timeout
  );
}

function fetchWithTimeout(url, options, timeout = 30000) {
  return Promise.race([
    fetch(url, options),
    new Promise((_, reject) =>
      setTimeout(() => reject(new Error("Timeout has passed")), timeout)
    )
  ]);
}
