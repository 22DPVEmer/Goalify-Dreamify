import axios from "axios";

const API_URL = "http://localhost:5129/api";

// Create axios instance with default config
const api = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true, // Using cookies
});

// Update response interceptor
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      window.location.href = "/login";
    }
    return Promise.reject(error);
  }
);

// Export interfaces
export interface Goal {
  id: string;
  title: string;
  description: string;
  dueDate: string | null;
  priority: "Low" | "Medium" | "High" | "Critical";
  isPublic: boolean;
  userId: string | null;
  isActive: boolean;
  isCompleted: boolean;
  status: "NotStarted" | "InProgress" | "Completed" | "OnHold" | "Cancelled";
  tags: { name: string }[];
  categories: Category[];
}

// Interface for raw goal data from backend
interface RawGoal extends Omit<Goal, "priority"> {
  priority: number;
}

export interface Category {
  id: string;
  name: string;
  description: string;
}

export const priorityToString = (
  priority: number
): "Low" | "Medium" | "High" | "Critical" => {
  switch (priority) {
    case 0:
      return "Low";
    case 1:
      return "Medium";
    case 2:
      return "High";
    case 3:
      return "Critical";
    default:
      return "Medium";
  }
};

export const statusToString = (
  status: number
): "NotStarted" | "InProgress" | "Completed" | "OnHold" | "Cancelled" => {
  switch (status) {
    case 0:
      return "NotStarted";
    case 1:
      return "InProgress";
    case 2:
      return "Completed";
    case 3:
      return "OnHold";
    case 4:
      return "Cancelled";
    default:
      return "NotStarted";
  }
};

const apiService = {
  getUserGoals: async (): Promise<Goal[]> => {
    const response = await api.get("/goal");
    console.log("Raw response data:", response.data);

    return response.data.map((goal: RawGoal) => {
      console.log("Processing goal:", goal.id);
      console.log("Original priority:", goal.priority);
      console.log("Original status:", goal.status);

      const processedGoal = {
        ...goal,
        tags: goal.tags || [],
        categories: goal.categories || [],
        dueDate: goal.dueDate ? new Date(goal.dueDate) : null,
        priority: priorityToString(goal.priority),
        status:
          typeof goal.status === "number"
            ? statusToString(goal.status)
            : goal.status || "NotStarted",
      };

      console.log("Processed priority:", processedGoal.priority);
      console.log("Processed status:", processedGoal.status);
      return processedGoal;
    });
  },

  getGoalById: async (id: string) => {
    const response = await api.get(`/goal/${id}`);
    const goal = response.data;
    return {
      ...goal,
      priority: priorityToString(goal.priority),
      status:
        typeof goal.status === "number"
          ? statusToString(goal.status)
          : goal.status || "NotStarted",
    };
  },

  createGoal: async (goalData: Partial<Goal>): Promise<Goal> => {
    const response = await api.post("/goal", goalData);
    return response.data;
  },

  updateGoal: async (id: string, goal: Goal): Promise<Goal> => {
    try {
      // Convert status to match backend enum format
      const statusMap = {
        NotStarted: 0,
        InProgress: 1,
        Completed: 2,
        OnHold: 3,
        Cancelled: 4,
      };

      // Convert the goal to a dictionary format for main update
      const goalData = {
        id: goal.id,
        title: goal.title,
        description: goal.description,
        dueDate: goal.dueDate,
        priority: goal.priority,
        status: statusMap[goal.status as keyof typeof statusMap],
        isPublic: goal.isPublic,
        userId: goal.userId,
        isActive: goal.isActive,
        isCompleted: goal.isCompleted,
      };

      console.log("Sending goal data to backend:", goalData);

      // First update the main goal data
      const response = await api.put(`/goal/${id}`, goalData);

      // Update tags if present
      if (goal.tags && goal.tags.length > 0) {
        const tagNames = goal.tags.map((tag) => tag.name);
        await api.put(`/goal/${id}/tags`, tagNames);
      }

      // Update category - since we can only have one category
      if (goal.categories && goal.categories.length > 0) {
        const categoryId = goal.categories[0].id; // Get the first (and only) category
        await api.put(`/goal/${id}/categories`, categoryId);
      } else {
        // If no categories, send empty string to clear any existing category
        await api.put(`/goal/${id}/categories`, "");
      }

      return response.data;
    } catch (error) {
      console.error("Error updating goal:", error);
      throw error;
    }
  },

  deleteGoal: async (id: string) => {
    await api.delete(`/goal/${id}`);
  },

  updateGoalPriority: async (id: string, priority: string) => {
    await api.patch(`/goal/${id}/priority`, priority);
  },

  updateGoalStatus: async (id: string, status: string): Promise<void> => {
    const statusMap = {
      NotStarted: 0,
      InProgress: 1,
      Completed: 2,
      OnHold: 3,
      Cancelled: 4,
    };

    console.log("Status update request:", {
      id,
      status,
      availableStatuses: Object.keys(statusMap),
    });

    // Normalize the status string to match our map
    const normalizedStatus = status.replace(/[-\s]/g, "");
    const numericStatus = statusMap[normalizedStatus as keyof typeof statusMap];

    console.log("Status conversion:", {
      originalStatus: status,
      normalizedStatus,
      numericStatus,
    });

    if (numericStatus === undefined) {
      console.error("Invalid status value:", status);
      throw new Error(
        `Invalid status value: ${status}. Valid values are: ${Object.keys(
          statusMap
        ).join(", ")}`
      );
    }

    try {
      const response = await api.patch(
        `/goal/${id}/status`,
        { value: numericStatus },
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      console.log("Status update response:", response.data);
    } catch (error) {
      console.error("Status update error:", error);
      console.error("Request details:", {
        url: `/goal/${id}/status`,
        payload: { value: numericStatus },
        status,
        normalizedStatus,
        numericStatus,
      });
      throw error;
    }
  },

  addTags: async (goalId: string, tags: string[]) => {
    await api.post(`/goal/${goalId}/tags`, tags);
  },

  getCategories: async (): Promise<Category[]> => {
    const response = await api.get<Category[]>("/category");
    return response.data;
  },

  addCategory: async (category: Omit<Category, "id">) => {
    const response = await api.post("/categories", category);
    return response.data;
  },

  addGoalToCategory: async (goalId: string, categoryId: string) => {
    await api.post(`/goal/${goalId}/categories/${categoryId}`);
  },

  createCategory: async (
    categoryData: Omit<Category, "id" | "createdAt" | "updatedAt">
  ): Promise<Category> => {
    const response = await api.post<Category>(`/api/category`, categoryData);
    return response.data;
  },

  updateCategory: async (
    id: string,
    categoryData: Partial<Omit<Category, "id">>
  ): Promise<void> => {
    await api.put(`/api/category/${id}`, categoryData);
  },

  deleteCategory: async (id: string): Promise<void> => {
    await api.delete(`/api/category/${id}`);
  },

  updateGoalTags: async (goalId: string, tags: string[]) => {
    await api.put(`/goal/${goalId}/tags`, tags);
  },

  updateGoalCategories: async (goalId: string, categoryIds: string[]) => {
    // Send only the first category ID or empty string if no category selected
    const categoryId = categoryIds.length > 0 ? categoryIds[0] : "";
    await api.put(`/goal/${goalId}/categories`, categoryId);
  },

  removeGoalFromCategory: async (goalId: string, categoryId: string) => {
    await api.delete(`/goal/${goalId}/categories/${categoryId}`);
  },
};

export default apiService;
