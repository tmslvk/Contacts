<template>
  <div class="card my-5 contact-card">
    <header class="card-header is-flex is-justify-content-space-between is-align-items-center px-4 py-3">
      <div class="card-header-title p-0 is-flex is-flex-direction-column">
        <h3 class="title is-3 mb-1">
          {{ contact.firstName }} {{ contact.lastName }}
        </h3>
        <p />
        <p class="subtitle is-6 mb-0">{{ contact.jobTitle || "—" }}</p>
      </div>

      <div
        class="buttons mr-3"
        style="margin-bottom: 0"
      >
        <button
          class="button is-white is-small"
          @click="editContact"
          title="Редактировать"
        >
          <span class="icon has-text-info">
            <i class="fas fa-pen"></i>
          </span>
        </button>

        <button
          class="button is-white is-small"
          @click="deleteContact"
          title="Удалить"
        >
          <span class="icon has-text-danger">
            <i class="fas fa-trash"></i>
          </span>
        </button>
      </div>
    </header>
    <div class="card-content">
      <div class="content">
        <p><strong>Номера телефонов:</strong></p>
        <ul>
          <li
            v-for="(phone, index) in contact.phoneNumbers"
            :key="phone.id || index"
          >
            {{ phone.number }}
          </li>
        </ul>
        <p class="mt-3">
          <strong>Дата рождения:</strong>
          {{ formattedBirthDate || "—" }}
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { defineProps, defineEmits, computed } from "vue";
import axios from "axios";

const props = defineProps({
  contact: {
    type: Object,
    required: true,
  },
});

const emit = defineEmits(["edit", "deleted"]);

const formattedBirthDate = computed(() => {
  if (!props.contact.birthDate) return "";
  const date = new Date(props.contact.birthDate);
  return date.toLocaleDateString("ru-RU", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });
});

const editContact = () => {
  emit("edit", props.contact);
};

const deleteContact = async () => {
  const confirmed = window.confirm(
    "Вы уверены, что хотите удалить этот контакт?"
  );
  if (!confirmed) return;

  try {
    await axios.delete(`http://localhost:5152/api/contact/${props.contact.id}`);
    emit("deleted", props.contact.id);
  } catch (err) {
    console.error("Ошибка удаления контакта:", err);
    alert("Не удалось удалить контакт. Попробуйте позже.");
  }
};
</script>

<style scoped>
.contact-card {
  border: 1px solid #dbdbdb;
  border-radius: 8px;
}

.card-header-title h3 {
  margin-bottom: 0.25rem;
}

.card-header-title p {
  margin-bottom: 0;
}

ul {
  padding-left: 1.2rem;
}

li {
  list-style-type: disc;
}
</style>
