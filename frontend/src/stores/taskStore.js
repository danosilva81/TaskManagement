import { defineStore } from 'pinia'
import { taskApi } from '@/services/api'

export const useTaskStore = defineStore('task', {
  state: () => ({
    tasks: [],
    loading: false,
    error: null,
    currentFilter: 'all', // 'all', 'Todo', 'InProgress', 'Done'
  }),

  getters: {
    filteredTasks: (state) => {
      if (state.currentFilter === 'all') {
        return state.tasks
      }
      return state.tasks.filter((task) => task.status === state.currentFilter)
    },

    tasksByStatus: (state) => (status) => {
      return state.tasks.filter((task) => task.status === status)
    },

    taskCount: (state) => state.tasks.length,

    todoCount: (state) => state.tasks.filter((t) => t.status === 'Todo').length,
    inProgressCount: (state) => state.tasks.filter((t) => t.status === 'InProgress').length,
    doneCount: (state) => state.tasks.filter((t) => t.status === 'Done').length,
  },

  actions: {
    async fetchTasks() {
      this.loading = true
      this.error = null
      try {
        const response = await taskApi.getAllTasks()
        this.tasks = response.data
      } catch (error) {
        this.error = error.response?.data?.error || 'Failed to fetch tasks'
        console.error('Error fetching tasks:', error)
      } finally {
        this.loading = false
      }
    },

    async createTask(taskData) {
      this.loading = true
      this.error = null
      try {
        const response = await taskApi.createTask(taskData)
        this.tasks.push(response.data)
        return response.data
      } catch (error) {
        this.error = error.response?.data?.error || 'Failed to create task'
        console.error('Error creating task:', error)
        throw error
      } finally {
        this.loading = false
      }
    },

    async updateTask(id, taskData) {
      this.loading = true
      this.error = null
      try {
        const dataToSend = { ...taskData }
        if (typeof dataToSend.status === 'string') {
          const statusMap = {
            Todo: 0,
            InProgress: 1,
            Done: 2,
          }
          dataToSend.status = statusMap[dataToSend.status]
        }

        const response = await taskApi.updateTask(id, dataToSend)
        const index = this.tasks.findIndex((t) => t.id === id)
        if (index !== -1) {
          this.tasks[index] = response.data
        }
        return response.data
      } catch (error) {
        this.error = error.response?.data?.error || 'Failed to update task'
        console.error('Error updating task:', error)
        throw error
      } finally {
        this.loading = false
      }
    },

    async deleteTask(id) {
      this.loading = true
      this.error = null
      try {
        await taskApi.deleteTask(id)
        this.tasks = this.tasks.filter((t) => t.id !== id)
      } catch (error) {
        this.error = error.response?.data?.error || 'Failed to delete task'
        console.error('Error deleting task:', error)
        throw error
      } finally {
        this.loading = false
      }
    },

    setFilter(filter) {
      this.currentFilter = filter
    },
  },
})
