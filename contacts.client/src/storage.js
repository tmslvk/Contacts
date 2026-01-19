import { createStore } from "vuex";

export default createStore({
    modules: {
        contacts: {
            namespaced: true,
            state: () => ({
                contacts: [],
                loading: false,
            }),
            mutations: {
                setContacts(state, contacts) {
                    state.contacts = contacts;
                },
                addContacts(state, contacts) {
                    state.contacts.push(...contacts);
                },
                setLoading(state, value) {
                    state.loading = value;
                },
            },
        },
    },
});
