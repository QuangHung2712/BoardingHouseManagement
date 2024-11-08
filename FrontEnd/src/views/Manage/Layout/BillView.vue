<template>
    <v-row class="filter">
        <v-col cols="3"><b class="text-h5">Hóa đơn</b></v-col>
        <v-col cols="3"><v-text-field clearable label="Tên khách thuê" v-model="customerName"></v-text-field></v-col>
        <v-col cols="3"><v-text-field clearable label="SĐT khách thuê" v-model="phoneNumber"></v-text-field></v-col>
        <v-col cols="3"><v-text-field clearable label="Số phòng" v-model="RoomNumber"></v-text-field></v-col>
    </v-row>
    <v-data-table 
    :headers="headers"
    :items="dataContract"
    :items-per-page="10"
    class="border-sm rounded-xl">
        <template v-slot:[`item.stt`]="{ index }">
                <!-- Hiển thị số thứ tự, cộng thêm 1 vì index bắt đầu từ 0 -->
            {{ index + 1 }}
        </template>
        <template v-slot:[`item.actions`]="{ item }">
            <v-icon small @click="viewDetails(item.id)">mdi-eye</v-icon>
            <v-icon class="ml-5" small @click="goToContractEdit(item.id)">mdi-pencil-circle </v-icon>
            <v-icon class="ml-5" small @click="btnDeleteContract(item.id,item.RoomName)">mdi-delete-empty </v-icon>
        </template>
    </v-data-table>'
</template>
<script>
    export default {
        data(){
            return{
                headers:[
                    {title: 'STT', value: 'stt'},
                    {title: 'Tên khách thuê',value: 'customerName'},
                    {title: 'Số phòng', value: 'RoomName'},
                    {title: 'Số điện thoại',value: 'phoneNumber'},
                    {title:'Số tiền', value: 'Amount'},
                    {title:'Tháng', value: 'Month'},
                    { title: 'Actions', value: 'actions', sortable: false },
                ],
            }
        }
    }
</script>