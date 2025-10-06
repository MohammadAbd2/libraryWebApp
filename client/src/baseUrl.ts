const isProduction = import.meta.env.PROD;

const prod = "https://server-small-dawn-3376.fly.dev";
const dev = "http://localhost:8080/";
export const dev_backend_url = "http://localhost:8080";

export const finalUrl = isProduction ? prod : dev;
