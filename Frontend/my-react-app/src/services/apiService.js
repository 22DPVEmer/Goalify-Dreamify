import axios from "axios";

const API_URL = "http://localhost:5129/api"; // Adjust the URL as needed

const apiService = {
  getUserGoals: async (userId) => {
    const response = await axios.get(`${API_URL}/goal`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
    return response.data;
  },

  getGoalById: async (id) => {
    const response = await axios.get(`${API_URL}/goal/${id}`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
    return response.data;
  },

  createGoal: async (goal) => {
    const response = await axios.post(`${API_URL}/goal`, goal, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
    return response.data;
  },

  updateGoal: async (id, goal) => {
    const response = await axios.put(`${API_URL}/goal/${id}`, goal, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
    return response.data;
  },

  deleteGoal: async (id) => {
    await axios.delete(`${API_URL}/goal/${id}`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
  },

  updateGoalPriority: async (id, priority) => {
    await axios.patch(`${API_URL}/goal/${id}/priority`, priority, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
  },

  addTags: async (id, tags) => {
    await axios.post(`${API_URL}/goal/${id}/tags`, tags, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
  },
};

export default apiService;
