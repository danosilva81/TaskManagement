import axios from 'axios'

const API_BASE_URL = 'http://localhost:5185/api' 

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

export const taskApi = {
  // Get all tasks
  getAllTasks() {
    return api.get('/tasks')
  },

  // Get tasks by status
  getTasksByStatus(status) {
    return api.get(`/tasks/status/${status}`)
  },

  // Get single task
  getTaskById(id) {
    return api.get(`/tasks/${id}`)
  },

  // Create task
  createTask(task) {
    return api.post('/tasks', task)
  },

  // Update task
  updateTask(id, task) {
    return api.put(`/tasks/${id}`, task)
  },

  // Delete task
  deleteTask(id) {
    return api.delete(`/tasks/${id}`)
  }
}

export default api