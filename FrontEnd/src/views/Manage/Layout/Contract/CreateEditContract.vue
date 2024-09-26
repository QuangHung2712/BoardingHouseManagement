<template>
    <V-row>
        <v-col cols="4">
            <h1>{{ title }}</h1>
        </v-col>
        <v-col offset="4" col="4" class="d-flex justify-space-between mt-2">
            <v-btn @click="btnBack()">Quay về</v-btn>
            <v-btn>Lưu</v-btn>
        </v-col>
    </V-row>
    <v-tabs v-model="btn">
        <v-tab :class="btn === 0 ? 'active-tab' : 'inactive-tab'">Thông tin khách thuê</v-tab>
      <v-tab :class="btn === 1 ? 'active-tab' : 'inactive-tab'">Dịch Vụ</v-tab>
      <v-tab :class="btn === 2 ? 'active-tab' : 'inactive-tab'">Hợp đồng</v-tab>
    </v-tabs>
    <v-tabs-items v-model="btn" class="">
      <v-tab-item >
        <Comtusmer v-show="btn === 0" class="ml-1"></Comtusmer>
      </v-tab-item>
      <v-tab-item>
        <ServiceView v-show="btn === 1"></ServiceView>
      </v-tab-item>
    </v-tabs-items>
</template>
<script>
import Comtusmer from '@/components/Contract/CustomerEditView.vue';
import ServiceView from '@/components/Contract/ServiceEditView.vue';

    export default{
  components: { Comtusmer,ServiceView },
        data(){
            return{
                title: 'Thêm hợp đồng',
                btn: 0,
            }
        },
        methods:{
            btnBack(){
                this.$router.back();
            }
        }
    }
</script>
<style scoped>
.active-tab {
  background-color: #4caf50 !important; /* Màu nền cho tab được chọn */
  color: white !important; /* Màu chữ trắng */
}
.inactive-tab {
  background-color: #eeeeee !important; /* Màu nền cho tab không được chọn */
  color: black !important; /* Màu chữ đen */
}
</style>