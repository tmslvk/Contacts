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
        <p class="modal-card-title">Редактировать контакт</p>
        <button
          class="delete"
          @click="close"
        ></button>
      </header>

      <section class="modal-card-body">
        <!-- Имя -->
        <div class="field">
          <label class="label">Имя</label>
          <input
            class="input"
            v-model="form.firstName"
          />
        </div>

        <!-- Фамилия -->
        <div class="field">
          <label class="label">Фамилия</label>
          <input
            class="input"
            v-model="form.lastName"
          />
        </div>

        <!-- Номера телефонов -->
        <div class="field">
          <label class="label">Номера телефонов</label>
          <div
            v-for="(phone, index) in form.phoneNumbers"
            :key="index"
            class="is-flex mb-2"
          >
            <input
              class="input"
              v-model="form.phoneNumbers[index].number"
              :placeholder="`Номер телефона ${index + 1}`"
            />
            <button
              class="button is-small is-danger ml-2"
              @click="removePhoneNumber(index)"
            >
              Удалить
            </button>
          </div>
          <p
            class="help is-danger"
            v-if="phoneError"
          >{{ phoneError }}</p>
          <button
            class="button is-small is-link mt-2"
            @click="addPhoneNumber"
            :disabled="form.phoneNumbers.length >= 3"
          >
            Добавить номер телефона
          </button>
        </div>

        <!-- Должность -->
        <div class="field">
          <label class="label">Должность</label>
          <input
            class="input"
            v-model="form.jobTitle"
          />
        </div>

        <!-- Дата рождения -->
        <div class="field">
          <label class="label">Дата рождения</label>
          <Datepicker
            v-model="form.birthDate"
            :format="customDateFormat"
            placeholder="Выберите дату"
          />
        </div>

        <!-- Ошибки -->
        <p
          class="has-text-danger mt-2"
          v-if="errorMessage"
        >{{ errorMessage }}</p>
      </section>

      <footer class="modal-card-foot">
        <button
          class="button is-success"
          @click="save"
        >Сохранить</button>
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
import { parseISO } from "date-fns";

const props = defineProps({
  modelValue: Boolean,
  contact: Object,
});
const emit = defineEmits(["update:modelValue", "saved"]);

const customDateFormat = "dd.MM.yyyy";
const errorMessage = ref("");
const phoneError = ref("");

// Форма
const form = reactive({
  firstName: "",
  lastName: "",
  phoneNumbers: [],
  jobTitle: "",
  birthDate: null,
});

// Синхронизация с props.contact
watch(
  () => props.contact,
  (val) => {
    if (val) {
      form.firstName = val.firstName || "";
      form.lastName = val.lastName || "";
      form.jobTitle = val.jobTitle || "";
      form.phoneNumbers =
        val.phoneNumbers?.map((p) => ({ number: p.number || p })) || [];
      if (val.birthDate instanceof Date) {
        form.birthDate = val.birthDate;
      } else if (typeof val.birthDate === "string") {
        form.birthDate = parseISO(val.birthDate);
      } else {
        form.birthDate = null;
      }
    }
  },
  { immediate: true }
);

// Добавление/удаление телефона
const addPhoneNumber = () => {
  if (form.phoneNumbers.length >= 3) return;
  form.phoneNumbers.push({ number: "" });
};

const removePhoneNumber = (index) => {
  form.phoneNumbers.splice(index, 1);
};

// Валидация телефонов
const validatePhones = () => {
  phoneError.value = "";
  const phoneRegex = /^\+?\d{10,15}$/;
  for (const p of form.phoneNumbers) {
    if (!phoneRegex.test(p.number)) {
      phoneError.value = "Номер телефона должен быть в формате +XXXXXXXXXXX";
      return false;
    }
  }
  return true;
};

// Закрытие
const close = () => emit("update:modelValue", false);

// Сохранение
const save = async () => {
  errorMessage.value = "";

  if (!form.firstName || !form.lastName || !form.jobTitle || !form.birthDate) {
    errorMessage.value = "Заполните все обязательные поля.";
    return;
  }

  if (form.phoneNumbers.length === 0) {
    phoneError.value = "Добавьте хотя бы один номер телефона.";
    return;
  }

  if (!validatePhones()) return;

  if (!props.contact?.id) {
    errorMessage.value = "Невозможно сохранить: нет id контакта";
    return;
  }

  try {
    const payload = {
      firstName: form.firstName,
      lastName: form.lastName,
      phoneNumbers: form.phoneNumbers.map((p) => p.number), // <-- массив строк
      jobTitle: form.jobTitle,
      birthDate: form.birthDate.toISOString(),
    };

    await axios.put(
      `http://localhost:5152/api/contact/${props.contact.id}`,
      payload
    );

    // Отправляем полный объект, включая id и обновленные телефоны
    emit("saved", {
      ...props.contact,
      ...payload,
      phoneNumbers: form.phoneNumbers,
    });

    close();
  } catch (err) {
    console.error(err);
    errorMessage.value =
      "Ошибка: " + (err.response?.data?.title || "Неизвестная ошибка");
  }
};
</script>

<style scoped>
.modal-card {
  width: 600px;
  max-width: 95vw;
}
.help.is-danger {
  margin-top: 4px;
}
.ml-2 {
  margin-left: 8px;
}
</style>
