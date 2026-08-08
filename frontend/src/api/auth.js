const ROLE_CLAIM =
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

export const getToken = () => localStorage.getItem("token");

export const getRoleFromToken = (token = getToken()) => {
  if (!token) return "";
  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return payload[ROLE_CLAIM] || payload.role || "";
  } catch {
    return "";
  }
};

export const clearAuth = () => {
  localStorage.removeItem("token");
  localStorage.removeItem("userRole");
};
