import axios from "axios";

// Cấu hình axios với base URL
const apiClient = axios.create({
  baseURL: "http://localhost:8080", // URL gốc
  headers: {
    "Content-Type": "application/json",
    // Các headers khác nếu cần
  },
});

// Export apiClient để sử dụng trong toàn bộ ứng dụng
export default apiClient;