<template>
    <v-row class="m0 filter">
        <v-col cols="2" class="mt-3"><b class="text-h5">Hợp đồng</b></v-col>
        <v-col cols="2"><v-text-field clearable label="Tên khách hàng"></v-text-field></v-col>
        <v-col cols="2"><v-text-field clearable label="Số phòng"></v-text-field></v-col>
        <v-col cols="2"><v-text-field clearable label="SĐT khách hàng"></v-text-field></v-col>
        <v-col cols="2"><v-text-field clearable label="Số phòng"></v-text-field></v-col>
        <v-col cols="2" class="d-flex justify-center mt-2"><v-btn @click="goToContractEdit(0)" class=" rounded-xl">Thêm hợp đồng</v-btn></v-col>
    </v-row>
    <v-row class="m0 data">
        <v-data-table
        :headers="headers"
        :items="contracts">
            <template v-slot:[`item.actions`]="{ item }">
            <v-icon small @click="viewDetails(item)">mdi-eye</v-icon>
            <v-icon class="ml-5" small @click="viewEditContract()">mdi-pencil-circle </v-icon>
            <v-icon class="ml-5" small @click="viewEditContract()">mdi-delete-empty </v-icon>
            </template>
        </v-data-table>
    </v-row>
</template>
<script>
    export default{
        data(){
            return{
                headers:[
                    {title: 'Tên khách thuê',value: 'customerName'},
                    {title: 'Số phòng', value: 'RoomName'},
                    {title: 'Ngày thuê',value: 'startDate'},
                    {title:'Tiền cọc', value: 'Deposit'},
                    { title: 'Actions', value: 'actions', sortable: false },
                ],
                contracts: [
                    {id:1, customerId: 1, customerName: 'Phạm Quang Hưng', roomId: 1, RoomName : '102', startDate:'27/12/2024', Deposit: 2000000}
                ]
            }
        },
        methods:{
            viewEditContract(){
                alert('Buttom đã được click')
            },
            goToContractEdit(idcontract) {
                this.$router.push({ name: 'createEdit', params: { idcontract } });
            }
        }
    }
</script>
<style scoped>
</style>