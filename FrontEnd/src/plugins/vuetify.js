// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'


// Vuetify
import { createVuetify } from 'vuetify'
import { VDateInput } from 'vuetify/labs/VDateInput'
import { vi } from 'vuetify/locale';

export default createVuetify({
  locale: {
    current: 'vi',  // Thiết lập ngôn ngữ mặc định là tiếng Việt
    messages: {
      vi,  // Ngôn ngữ tiếng Việt từ Vuetify
    },
  },
  components: {
    VDateInput,
  },
  // https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides
})
