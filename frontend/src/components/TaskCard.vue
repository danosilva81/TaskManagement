<template>
  <div class="task-card" :class="`status-${task.status.toLowerCase()}`">
    <div class="task-header">
      <h3 class="task-title">{{ task.title }}</h3>
      <span class="task-status" :class="`badge-${task.status.toLowerCase()}`">
        {{ task.status }}
      </span>
    </div>
    
    <p v-if="task.description" class="task-description">
      {{ task.description }}
    </p>
    
    <div class="task-footer">
      <div class="task-dates">
        <small>Created: {{ formatDate(task.createdAt) }}</small>
        <small v-if="task.updatedAt">Updated: {{ formatDate(task.updatedAt) }}</small>
      </div>
      
      <div class="task-actions">
        <select 
          v-model="localStatus" 
          @change="updateStatus"
          class="status-select"
        >
          <option value="Todo">To Do</option>
          <option value="InProgress">In Progress</option>
          <option value="Done">Done</option>
        </select>
        
        <button @click="$emit('edit', task)" class="btn-edit" title="Edit">
          ✏️
        </button>
        <button @click="$emit('delete', task.id)" class="btn-delete" title="Delete">
          🗑️
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  task: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['update-status', 'edit', 'delete'])

const localStatus = ref(props.task.status)

watch(() => props.task.status, (newStatus) => {
  localStatus.value = newStatus
})

const updateStatus = () => {
  if (localStatus.value !== props.task.status) {
    const statusMap = {
      'Todo': 0,
      'InProgress': 1,
      'Done': 2
    }
    emit('update-status', props.task.id, statusMap[localStatus.value])
  }
}

const formatDate = (dateString) => {
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', { 
    month: 'short', 
    day: 'numeric', 
    year: 'numeric' 
  })
}
</script>

<style scoped>
.task-card {
  background: white;
  border-radius: 8px;
  padding: 1.25rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border-left: 4px solid #ddd;
  transition: all 0.3s ease;
}

.task-card:hover {
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
  transform: translateY(-2px);
}

.task-card.status-todo {
  border-left-color: #3b82f6;
}

.task-card.status-inprogress {
  border-left-color: #f59e0b;
}

.task-card.status-done {
  border-left-color: #10b981;
}

.task-header {
  display: flex;
  justify-content: space-between;
  align-items: start;
  margin-bottom: 0.75rem;
  gap: 1rem;
}

.task-title {
  margin: 0;
  font-size: 1.125rem;
  font-weight: 600;
  color: #1f2937;
  flex: 1;
}

.task-status {
  padding: 0.25rem 0.75rem;
  border-radius: 12px;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
  white-space: nowrap;
}

.badge-todo {
  background: #dbeafe;
  color: #1e40af;
}

.badge-inprogress {
  background: #fef3c7;
  color: #b45309;
}

.badge-done {
  background: #d1fae5;
  color: #065f46;
}

.task-description {
  color: #6b7280;
  font-size: 0.875rem;
  margin: 0 0 1rem 0;
  line-height: 1.5;
}

.task-footer {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  gap: 1rem;
  flex-wrap: wrap;
}

.task-dates {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.task-dates small {
  color: #9ca3af;
  font-size: 0.75rem;
}

.task-actions {
  display: flex;
  gap: 0.5rem;
  align-items: center;
}

.status-select {
  padding: 0.375rem 0.5rem;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 0.875rem;
  cursor: pointer;
  background: white;
  transition: border-color 0.2s;
}

.status-select:hover {
  border-color: #9ca3af;
}

.status-select:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.btn-edit,
.btn-delete {
  padding: 0.375rem 0.5rem;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-size: 1rem;
  transition: all 0.2s;
  background: transparent;
}

.btn-edit:hover {
  background: #f3f4f6;
}

.btn-delete:hover {
  background: #fee2e2;
}
</style>