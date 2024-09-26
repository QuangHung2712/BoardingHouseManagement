<template>
    <v-row class="filter">
        <v-col cols="2"><b class="text-h5">Hợp đồng</b></v-col>
        <v-col cols="2"><v-text-field clearable label="Tên khách hàng" v-model="customerName"></v-text-field></v-col>
        <v-col cols="2"><v-text-field clearable label="Ngày thuê" v-model="startDate"></v-text-field></v-col>
        <v-col cols="2"><v-text-field clearable label="SĐT khách hàng" v-model="customerSDT"></v-text-field></v-col>
        <v-col cols="2"><v-text-field clearable label="Số phòng" v-model="roomName"></v-text-field></v-col>
        <v-col cols="2" class="d-flex justify-center mt-2"><v-btn @click="goToContractEdit(0)" class=" rounded-xl">Thêm hợp đồng</v-btn></v-col>
    </v-row>
        <v-data-table 
        :headers="headers"
        :items="dataContract"
        :items-per-page="6"
        class="border-sm rounded-xl">
            <template v-slot:[`item.actions`]="{ item }">
            <v-icon small @click="viewDetails(item)">mdi-eye</v-icon>
            <v-icon class="ml-5" small @click="goToContractEdit(item.id)">mdi-pencil-circle </v-icon>
            <v-icon class="ml-5" small @click="viewEditContract()">mdi-delete-empty </v-icon>
            </template>
        </v-data-table>
</template>
<script>
    export default{
        data(){
            return{
                customerName: '',
                StartDate: '',
                customerSDT: '',
                roomName: '',
                headers:[
                    {title: 'Tên khách thuê',value: 'customerName'},
                    {title: 'Số phòng', value: 'RoomName'},
                    {title: 'Ngày thuê',value: 'startDate'},
                    {title:'Tiền cọc', value: 'Deposit'},
                    {title:'Số điện thoại', value: 'numberPhone'},
                    { title: 'Actions', value: 'actions', sortable: false },
                ],
                contracts: [
                    {id:1, customerId: 1, customerName: 'Phạm Quang Hưng', roomId: 1, RoomName : '102', startDate:'27/12/2024', Deposit: 2000000, numberPhone:'0359988934'},
                    {id:2, customerId: 1, customerName: 'Phạm Minh Trang', roomId: 1, RoomName : '103', startDate:'27/12/2024', Deposit: 2000000, numberPhone:'0984238731'},
                    {id:3, customerId: 1, customerName: 'Phạm Văn Thu', roomId: 1, RoomName : '202', startDate:'27/12/2024', Deposit: 2000000, numberPhone:'0886682304'},
                    {id:4, customerId: 1, customerName: 'Phạm Thị Bích Hà', roomId: 1, RoomName : '203', startDate:'27/12/2024', Deposit: 2000000, numberPhone:'0333378465'},
                    {id:5, customerId: 1, customerName: 'Phạm Quang Hưng', roomId: 1, RoomName : '204', startDate:'27/12/2024', Deposit: 2000000, numberPhone:'0359988934'},
                    {id:6, customerId: 1, customerName: 'Phạm Quang Hưng', roomId: 1, RoomName : '205', startDate:'27/12/2024', Deposit: 2000000, numberPhone:'0359988934'},
                    {id:7, customerId: 1, customerName: 'Phạm Quang Hưng', roomId: 1, RoomName : '303', startDate:'27/12/2024', Deposit: 2000000, numberPhone:'0359988934'},

                ]
            }
        },
        computed:{
            dataContract(){
                return this.contracts.filter((item) => {
                    const matchescustomerName = item.customerName.toLowerCase().includes(this.customerName.toLowerCase());
                    const matchesStartDate = item.startDate.toLowerCase().includes(this.StartDate.toLowerCase());
                    const matchescustomerSDT = item.numberPhone.toLowerCase().includes(this.customerSDT.toLowerCase());
                    const matchesroomName = item.RoomName.toLowerCase().includes(this.roomName.toLowerCase());
                    return matchescustomerName && matchesStartDate && matchescustomerSDT && matchesroomName;
                });
            }
        },
        mounted() {
          const idtower = this.$route.params.idtower;
          this.towerId = idtower
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
.filter .v-col{
    padding-bottom: 5px;
}
</style>