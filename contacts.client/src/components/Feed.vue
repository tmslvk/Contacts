<template>
  <div class="feed-page container mt-5">
    <!-- Фильтрация, поиск и кнопка создания -->
    <div class="field is-grouped is-grouped-multiline mb-4">
      <div class="control">
        <div class="select">
          <select
            v-model="sort"
            @change="resetFeed"
          >
            <option value="latest">Последние</option>
            <option value="old">Старые</option>
          </select>
        </div>
      </div>

      <div class="control">
        <input
          class="input"
          placeholder="Поиск..."
          v-model="searchQuery"
        />
      </div>

      <div class="control ml-auto">
        <button
          class="button is-primary"
          @click="openCreateModal"
        >
          Создать контакт
        </button>
      </div>
    </div>

    <section class="section feed-container">
      <div class="container">
        <transition-group
          name="fade"
          tag="div"
        >
          <!-- Если есть контакты после фильтра -->
          <template v-if="filteredContacts.length > 0">
            <ContactCard
              v-for="contact in filteredContacts"
              :key="contact.id"
              :contact="contact"
              @edit="openEditModal"
              @deleted="removeContact"
            />
          </template>

          <!-- Если ничего не найдено -->
          <p
            v-else
            class="has-text-centered mt-4"
          >
            Контакты не найдены.
          </p>
        </transition-group>
      </div>
    </section>

    <!-- Пагинация -->
    <div
      v-if="hasMore"
      class="has-text-centered mt-4"
    >
      <button
        class="button is-link"
        @click="loadContacts"
      >
        Загрузить еще
      </button>
    </div>

    <!-- Лоадер -->
    <div
      v-if="loading"
      class="has-text-centered my-4"
    >
      <span class="loader"></span>
    </div>

    <!-- Модальные окна -->
    <CreateContactModal
      v-if="modalVisible"
      :modelValue="modalVisible"
      :contact="selectedContact"
      @update:modelValue="modalVisible = $event"
      @posted="onContactPosted"
    />

    <EditContactModal
      v-if="editModalVisible"
      :modelValue="editModalVisible"
      :contact="selectedContact"
      @update:modelValue="editModalVisible = $event"
      @saved="onContactPosted"
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useStore } from "vuex";
import axios from "axios";
import { parseISO } from "date-fns";

import ContactCard from "./ContactCard.vue";
import CreateContactModal from "./CreateContactModal.vue";
import EditContactModal from "./EditContactModal.vue";

const store = useStore();
const sort = ref("latest");
const page = ref(1);
const pageSize = 10;
const hasMore = ref(true);
const modalVisible = ref(false);
const editModalVisible = ref(false);
const selectedContact = ref(null);
const searchQuery = ref("");

// Загрузка контактов с сервера
const loadContacts = async () => {
  if (!hasMore.value) return;
  store.commit("contacts/setLoading", true);

  try {
    const url = "http://localhost:5152/api/contact";
    const res = await axios.get(url, {
      params: { page: page.value, pageSize },
    });

    let contacts = Array.isArray(res.data) ? res.data : [];

    // Нормализация телефонов и даты
    contacts = contacts.map((c) => ({
      ...c,
      phoneNumbers:
        c.phoneNumbers?.map((p) =>
          typeof p === "string" ? { id: null, number: p } : p
        ) || [],
      birthDate: c.birthDate ? parseISO(c.birthDate) : null,
    }));

    if (page.value === 1) store.commit("contacts/setContacts", contacts);
    else store.commit("contacts/addContacts", contacts);

    hasMore.value = contacts.length === pageSize;
    page.value++;
  } catch (err) {
    console.error(err);
  } finally {
    store.commit("contacts/setLoading", false);
  }
};

// Фильтрация поиска
const filteredContacts = computed(() => {
  const list = store.state.contacts?.contacts || [];
  return list.filter((c) => {
    const query = searchQuery.value.toLowerCase();
    return (
      c.firstName.toLowerCase().includes(query) ||
      c.lastName.toLowerCase().includes(query) ||
      c.phoneNumbers.some((p) => p.number.toLowerCase().includes(query))
    );
  });
});

// Сброс фида
const resetFeed = () => {
  page.value = 1;
  hasMore.value = true;
  store.commit("contacts/setContacts", []);
  loadContacts();
};

// Открытие модалок
const openCreateModal = () => {
  selectedContact.value = null;
  modalVisible.value = true;
};

const openEditModal = (contact) => {
  selectedContact.value = contact;
  editModalVisible.value = true;
};

// Обновление/добавление контакта
const onContactPosted = (contact) => {
  contact.phoneNumbers =
    contact.phoneNumbers?.map((p) =>
      typeof p === "string" ? { id: null, number: p } : p
    ) || [];
  contact.birthDate = contact.birthDate ? parseISO(contact.birthDate) : null;

  const list = store.state.contacts.contacts;
  const idx = list.findIndex((c) => c.id === contact.id);
  if (idx >= 0) list[idx] = contact;
  else list.unshift(contact);

  store.commit("contacts/setContacts", [...list]);

  modalVisible.value = false;
  editModalVisible.value = false;
};

// Удаление контакта
const removeContact = (id) => {
  store.commit(
    "contacts/setContacts",
    store.state.contacts.contacts.filter((c) => c.id !== id)
  );
};

// Статус загрузки
const loading = computed(() => store.state.contacts?.loading ?? false);

onMounted(() => {
  loadContacts();
  // SSE/WebSocket для real-time обновления
  // const evtSource = new EventSource("http://localhost:5152/api/contact/stream");
  // evtSource.onmessage = (e) => onContactPosted(JSON.parse(e.data));
});
</script>

<style scoped>
.feed-page {
  max-width: 800px;
  margin: 0 auto;
}

.loader {
  border: 4px solid #f3f3f3;
  border-top: 4px solid #3273dc;
  border-radius: 50%;
  width: 24px;
  height: 24px;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

/* Feed.vue */
.feed-page {
  width: 100%;
  max-width: 1400px; /* Увеличили максимально допустимую ширину */
  margin: 0 auto;
  background: white;
  border-radius: 10px;
  padding: 30px; /* побольше внутренних отступов */
  box-sizing: border-box;
  min-height: 80vh;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.25);
}

/* Контейнеры внутри Feed */
.feed-container .container {
  width: 100%;
}

/* Карточки контактов */
.contact-card {
  width: 100%;
  margin-bottom: 20px;
}

/* Для модалок */
.modal-card {
  width: 80vw; /* ширина модалки 80% от экрана */
  max-width: 800px; /* не больше 800px */
}

.fade-enter-active,
.fade-leave-active {
  transition: all 0.5s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(20px);
}
.fade-enter-to,
.fade-leave-from {
  opacity: 1;
  transform: translateY(0);
}
</style>
