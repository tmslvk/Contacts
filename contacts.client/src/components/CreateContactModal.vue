<template>
  <div
    class="modal"
    :class="{ 'is-active': modelValue }"
  >
    <div
      class="modal-background"
      @click="close"
    ></div>

    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">
          {{ props.contact?.id ? "Редактировать контакт" : "Создать контакт" }}
        </p>
        <button
          class="delete"
          @click="close"
        ></button>
      </header>

      <section class="modal-card-body">
        <!-- Имя -->
        <div class="field">
          <label class="label">Имя</label>
          <div class="control">
            <input
              v-model="contact.firstName"
              class="input"
            />
          </div>
        </div>

        <!-- Фамилия -->
        <div class="field">
          <label class="label">Фамилия</label>
          <div class="control">
            <input
              v-model="contact.lastName"
              class="input"
            />
          </div>
        </div>

        <!-- Должность -->
        <div class="field">
          <label class="label">Должность</label>
          <div class="control">
            <input
              v-model="contact.jobTitle"
              class="input"
            />
          </div>
        </div>

        <!-- Номер телефона -->
        <div class="field">
          <label class="label">Номер телефона</label>
          <div class="control">
            <input
              v-model="contact.phoneNumber"
              class="input"
            />
          </div>
        </div>

        <!-- Дата рождения -->
        <div class="field">
          <label class="label">Дата рождения</label>
          <div class="control">
            <Datepicker
              v-model="contact.birthDate"
              :format="customDateFormat"
              placeholder="Выберите дату"
            />
          </div>
        </div>

        <!-- Ошибки -->
        <p
          v-if="errorMessage"
          class="has-text-danger"
        >{{ errorMessage }}</p>
      </section>

      <footer class="modal-card-foot">
        <button
          class="button is-success"
          @click="submit"
        >
          {{ props.contact?.id ? "Сохранить" : "Опубликовать" }}
        </button>
        <button
          class="button"
          @click="close"
        >Отмена</button>
      </footer>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref, watch } from "vue";
import axios from "axios";
import Datepicker from "vue3-datepicker";

// Props и emits
const props = defineProps({
  modelValue: Boolean,
  contact: Object, // объект контакта для редактирования
});
const emit = defineEmits(["update:modelValue", "posted"]);

// Ошибки формы
const errorMessage = ref("");

// Данные формы (reactive)
const contact = reactive({
  firstName: "",
  lastName: "",
  jobTitle: "",
  phoneNumber: "",
  birthDate: null,
});

// Синхронизация формы с props.contact
watch(
  () => props.contact,
  (newVal) => {
    if (newVal) {
      Object.assign(contact, {
        firstName: newVal.firstName || "",
        lastName: newVal.lastName || "",
        jobTitle: newVal.jobTitle || "",
        phoneNumber: newVal.phoneNumbers?.[0] || "",
        birthDate: newVal.birthDate || null,
      });
    } else {
      Object.assign(contact, {
        firstName: "",
        lastName: "",
        jobTitle: "",
        phoneNumber: "",
        birthDate: null,
      });
    }
  },
  { immediate: true }
);

// Форматирование даты для Datepicker
const customDateFormat = (date) => {
  if (!date) return "";
  const d = new Date(date);
  return d.toLocaleDateString("ru-RU", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });
};

// Закрытие модалки
const close = () => emit("update:modelValue", false);

// Валидация формы
const validateForm = () => {
  if (
    !contact.firstName ||
    !contact.lastName ||
    !contact.jobTitle ||
    !contact.phoneNumber ||
    !contact.birthDate
  ) {
    errorMessage.value = "Заполните все обязательные поля.";
    return false;
  }
  errorMessage.value = "";
  return true;
};

// Отправка формы на сервер
const submit = async () => {
  if (!validateForm()) return;

  const payload = {
    firstName: contact.firstName,
    lastName: contact.lastName,
    phoneNumbers: [contact.phoneNumber],
    jobTitle: contact.jobTitle,
    birthDate: contact.birthDate,
  };

  try {
    const url = props.contact?.id
      ? `http://localhost:5152/api/contact/update/${props.contact.id}`
      : "http://localhost:5152/api/contact";

    const res = await axios.post(url, payload);
    emit("posted", res.data);
    close();
  } catch (err) {
    errorMessage.value =
      "Ошибка: " + (err.response?.data || "Неизвестная ошибка");
    console.error(err);
  }
};
</script>

<style scoped>
.modal-card {
  width: 600px;
  max-width: 95vw;
}
</style>
