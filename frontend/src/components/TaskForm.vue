<template>
  <div class="task-form-container">
    <div class="modal-overlay" @click="$emit('close')"></div>
    
    <div class="task-form-card">
      <div class="form-header">
        <h2>{{ isEditMode ? 'Edit Task' : 'Create New Task' }}</h2>
        <button @click="$emit('close')" class="btn-close">✕</button>
      </div>
      
      <form @submit.prevent="handleSubmit">
        <div class="form-group">
          <label for="title">Title *</label>
          <input
            id="title"
            v-model="formData.title"
            type="text"
            placeholder="Enter task title"
            required
            maxlength="250"
          />
          <small class="char-count">{{ formData.title.length }}/250</small>
        </div>
        
        <div class="form-group">
          <label for="description">Description</label>
          <textarea
            id="description"
            v-model="formData.description"
            placeholder="Enter task description (optional)"
            rows="4"
            maxlength="1000"
          ></textarea>
          <small class="char-count">{{ formData.description?.length || 0 }}/1000</small>
        </div>
        
        <div class="form-group" v-if="isEditMode">
          <label for="status">Status</label>
          <select id="status" v-model="formData.status">
            <option value="Todo">To Do</option>
            <option value="InProgress">In Progress</option>
            <option value="Done">Done</option>
          </select>
        </div>
        
        <div v-if="error" class="error-message">
          {{ error }}
        </div>
        
        <div class="form-actions">
          <button type="button" @click="$emit('close')" class="btn-secondary">
            Cancel
          </button>
          <button type="submit" class="btn-primary" :disabled="loading">
            {{ loading ? 'Saving...' : (isEditMode ? 'Update Task' : 'Create Task') }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  task: {
    type: Object,
    default: null
  },
  loading: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['submit', 'close'])

const isEditMode = ref(!!props.task)
const error = ref(null)

const formData = ref({
  title: props.task?.title || '',
  description: props.task?.description || '',
  status: props.task?.status || 'Todo'
})

watch(() => props.task, (newTask) => {
  if (newTask) {
    formData.value = {
      title: newTask.title || '',
      description: newTask.description || '',
      status: newTask.status || 'Todo'
    }
    isEditMode.value = true
  } else {
    formData.value = {
      title: '',
      description: '',
      status: 'Todo'
    }
    isEditMode.value = false
  }
})

const handleSubmit = () => {
  error.value = null
  
  if (!formData.value.title.trim()) {
    error.value = 'Title is required'
    return
  }
  
  const submitData = {
    title: formData.value.title.trim(),
    description: formData.value.description?.trim() || null
  }
  
  if (isEditMode.value) {
     const statusMap = {
      'Todo': 0,
      'InProgress': 1,
      'Done': 2
    }
    submitData.status = statusMap[formData.value.status]
  }
  
  emit('submit', submitData)
}
</script>

<style scoped>
.task-form-container {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
}

.task-form-card {
  position: relative;
  background: white;
  border-radius: 12px;
  padding: 2rem;
  width: 90%;
  max-width: 500px;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
}

.form-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.form-header h2 {
  margin: 0;
  color: #1f2937;
  font-size: 1.5rem;
}

.btn-close {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
  color: #6b7280;
  padding: 0.25rem;
  line-height: 1;
  transition: color 0.2s;
}

.btn-close:hover {
  color: #1f2937;
}

.form-group {
  margin-bottom: 1.25rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  color: #374151;
  font-weight: 500;
  font-size: 0.875rem;
}

.form-group input,
.form-group textarea,
.form-group select {
  width: 100%;
  padding: 0.625rem 0.75rem;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 0.875rem;
  font-family: inherit;
  transition: border-color 0.2s, box-shadow 0.2s;
}

.form-group input:focus,
.form-group textarea:focus,
.form-group select:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.form-group textarea {
  resize: vertical;
}

.char-count {
  display: block;
  text-align: right;
  color: #9ca3af;
  font-size: 0.75rem;
  margin-top: 0.25rem;
}

.error-message {
  background: #fee2e2;
  color: #dc2626;
  padding: 0.75rem;
  border-radius: 6px;
  font-size: 0.875rem;
  margin-bottom: 1rem;
}

.form-actions {
  display: flex;
  gap: 0.75rem;
  justify-content: flex-end;
  margin-top: 1.5rem;
}

.btn-primary,
.btn-secondary {
  padding: 0.625rem 1.25rem;
  border-radius: 6px;
  font-size: 0.875rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
  border: none;
}

.btn-primary {
  background: #3b82f6;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #2563eb;
}

.btn-primary:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-secondary {
  background: #f3f4f6;
  color: #374151;
}

.btn-secondary:hover {
  background: #e5e7eb;
}
</style>