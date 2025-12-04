<template>
  <div class="tasks-view">
    <div class="tasks-header">
      <div>
        <h1>Task Management</h1>
        <p class="subtitle">Organize and track your tasks efficiently</p>
      </div>
      <button @click="showCreateForm = true" class="btn-create">
        + New Task
      </button>
    </div>
    
    <!-- Stats Cards -->
    <div class="stats-grid">
      <div class="stat-card stat-total">
        <div class="stat-value">{{ taskStore.taskCount }}</div>
        <div class="stat-label">Total Tasks</div>
      </div>
      <div class="stat-card stat-todo">
        <div class="stat-value">{{ taskStore.todoCount }}</div>
        <div class="stat-label">To Do</div>
      </div>
      <div class="stat-card stat-progress">
        <div class="stat-value">{{ taskStore.inProgressCount }}</div>
        <div class="stat-label">In Progress</div>
      </div>
      <div class="stat-card stat-done">
        <div class="stat-value">{{ taskStore.doneCount }}</div>
        <div class="stat-label">Done</div>
      </div>
    </div>
    
    <!-- Filter Buttons -->
    <div class="filter-bar">
      <button
        v-for="filter in filters"
        :key="filter.value"
        @click="taskStore.setFilter(filter.value)"
        :class="['filter-btn', { active: taskStore.currentFilter === filter.value }]"
      >
        {{ filter.label }}
      </button>
    </div>
    
    <!-- Loading State -->
    <div v-if="taskStore.loading" class="loading">
      <div class="spinner"></div>
      <p>Loading tasks...</p>
    </div>
    
    <!-- Error State -->
    <div v-else-if="taskStore.error" class="error-container">
      <p>{{ taskStore.error }}</p>
      <button @click="taskStore.fetchTasks()" class="btn-retry">Retry</button>
    </div>
    
    <!-- Empty State -->
    <div v-else-if="taskStore.filteredTasks.length === 0" class="empty-state">
      <div class="empty-icon">📋</div>
      <h3>No tasks found</h3>
      <p>{{ taskStore.currentFilter === 'all' ? 'Create your first task to get started!' : `No ${taskStore.currentFilter} tasks` }}</p>
      <button v-if="taskStore.currentFilter === 'all'" @click="showCreateForm = true" class="btn-create">
        Create Task
      </button>
    </div>
    
    <!-- Tasks Grid -->
    <div v-else class="tasks-grid">
      <TaskCard
        v-for="task in taskStore.filteredTasks"
        :key="task.id"
        :task="task"
        @update-status="handleUpdateStatus"
        @edit="handleEdit"
        @delete="handleDelete"
      />
    </div>
    
    <!-- Create/Edit Form Modal -->
    <TaskForm
      v-if="showCreateForm || editingTask"
      :task="editingTask"
      :loading="taskStore.loading"
      @submit="handleSubmit"
      @close="closeForm"
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useTaskStore } from '@/stores/taskStore'
import TaskCard from '@/components/TaskCard.vue'
import TaskForm from '@/components/TaskForm.vue'

const taskStore = useTaskStore()
const showCreateForm = ref(false)
const editingTask = ref(null)

const filters = [
  { label: 'All', value: 'all' },
  { label: 'To Do', value: 'Todo' },
  { label: 'In Progress', value: 'InProgress' },
  { label: 'Done', value: 'Done' }
]

onMounted(() => {
  taskStore.fetchTasks()
})

const handleSubmit = async (taskData) => {
  try {
    if (editingTask.value) {
      await taskStore.updateTask(editingTask.value.id, taskData)
    } else {
      await taskStore.createTask(taskData)
    }
    closeForm()
  } catch (error) {
    console.error('Error submitting task:', error)
  }
}

const handleUpdateStatus = async (taskId, newStatus) => {
  try {
    await taskStore.updateTask(taskId, { status: newStatus })
  } catch (error) {
    console.error('Error updating status:', error)
  }
}

const handleEdit = (task) => {
  editingTask.value = task
}

const handleDelete = async (taskId) => {
  if (confirm('Are you sure you want to delete this task?')) {
    try {
      await taskStore.deleteTask(taskId)
    } catch (error) {
      console.error('Error deleting task:', error)
    }
  }
}

const closeForm = () => {
  showCreateForm.value = false
  editingTask.value = null
}
</script>

<style scoped>
.tasks-view {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.tasks-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  flex-wrap: wrap;
  gap: 1rem;
}

.tasks-header h1 {
  margin: 0;
  color: #1f2937;
  font-size: 2rem;
}

.subtitle {
  margin: 0.25rem 0 0 0;
  color: #6b7280;
  font-size: 0.875rem;
}

.btn-create {
  background: #3b82f6;
  color: white;
  border: none;
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  font-size: 0.875rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-create:hover {
  background: #2563eb;
  transform: translateY(-1px);
  box-shadow: 0 4px 6px rgba(59, 130, 246, 0.3);
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
  margin-bottom: 2rem;
}

.stat-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border-left: 4px solid;
}

.stat-total { border-left-color: #6b7280; }
.stat-todo { border-left-color: #3b82f6; }
.stat-progress { border-left-color: #f59e0b; }
.stat-done { border-left-color: #10b981; }

.stat-value {
  font-size: 2rem;
  font-weight: 700;
  color: #1f2937;
  margin-bottom: 0.25rem;
}

.stat-label {
  color: #6b7280;
  font-size: 0.875rem;
  font-weight: 500;
}

.filter-bar {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 2rem;
  flex-wrap: wrap;
}

.filter-btn {
  padding: 0.5rem 1rem;
  border: 2px solid #e5e7eb;
  background: white;
  border-radius: 8px;
  cursor: pointer;
  font-size: 0.875rem;
  font-weight: 500;
  color: #6b7280;
  transition: all 0.2s;
}

.filter-btn:hover {
  border-color: #3b82f6;
  color: #3b82f6;
}

.filter-btn.active {
  background: #3b82f6;
  border-color: #3b82f6;
  color: white;
}

.loading {
  text-align: center;
  padding: 4rem 2rem;
}

.spinner {
  width: 48px;
  height: 48px;
  border: 4px solid #e5e7eb;
  border-top-color: #3b82f6;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 1rem;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.error-container {
  text-align: center;
  padding: 4rem 2rem;
  color: #dc2626;
}

.btn-retry {
  margin-top: 1rem;
  padding: 0.5rem 1rem;
  background: #3b82f6;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}

.empty-state {
  text-align: center;
  padding: 4rem 2rem;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
}

.empty-state h3 {
  color: #1f2937;
  margin: 0 0 0.5rem 0;
}

.empty-state p {
  color: #6b7280;
  margin-bottom: 1.5rem;
}

.tasks-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 1.25rem;
}

@media (max-width: 768px) {
  .tasks-grid {
    grid-template-columns: 1fr;
  }
}
</style>