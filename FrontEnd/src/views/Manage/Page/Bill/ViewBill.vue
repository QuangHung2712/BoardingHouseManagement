<script>
import apiClient from '@/plugins/axios';


    export default {
        name: "Enter-Bill-Information",
        components: {
        },
        created(){

        },
        data(){
            return{
                IsBill: false,
                Email: '',
                billData:[

                ],
                message: '',
                snackbar: false,
                snackbarColor: '',
                headersTable:[
                    {title: 'STT', value: 'stt',sortable: true},
                    {title: 'Địa chỉ phòng',value:'addressTower',sortable: true},
                    {title: 'Số phòng',value: 'roomName',sortable: true},
                    {title: 'Tháng',value: 'time',sortable: true},
                    {title: 'Thời gian thanh toán',value: 'paymentDate',sortable: true},
                    {title: 'Số tiền',value: 'amount',sortable: true},
                ],
            }
        },
        computed: {
        },
        methods:{
            searchBillByEmail(){
                apiClient.get(`/Customer/GetBillByEmail?email=${this.Email}`)
                        .then(response=>{
                            this.billData = response.data;
                            console.log(this.billData);
                        })
                        .catch(error=>{
                            this.message = "Lấy danh sách hoá đơn bị lỗi: " + error.response?.data?.message || error.message;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
            },
            ViewBillDetail(row,index){
                this.$router.push({ name: 'EnterBill', params: { idbill: index.item.billID } });
            }
        }
    }
</script>
<template>
    <v-container>
        <v-snackbar
            v-model="snackbar"
            :timeout="10000"
            class="custom-snackbar"
            :color="snackbarColor"
        >
            <h5 class="text-center">{{ message }}</h5>
        </v-snackbar>
        <BCard>
            <BCardHeader >
                <h3 class="text-center">Xem hoá đơn theo Email</h3>
                <BRow>
                    <BCol class="col-lg-8">
                        <div class="form-group">
                            <label class="form-label">Địa chỉ Email:</label>
                            <v-text-field v-model="Email"  :rules="[required]" variant="outlined" clearable placeholder="Nhập vào địa chỉ Email của bạn" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-4 d-flex align-items-center">
                        <v-btn @click="searchBillByEmail()" v-show="!IsBill">Xem hoá đơn</v-btn>
                    </BCol>
                </BRow>
            </BCardHeader>
            <BCardBody>
                <h5 class="text-center">Danh sách hoá đơn</h5>
                <v-data-table 
                    :headers = "headersTable"
                    :items="billData"
                    item-value="roomName"
                    item-class="rowYellow"
                    class="border-sm rounded-lg"
                    @click:row="ViewBillDetail"
                    >
                    <template v-slot:[`item.stt`]="{ index }">
                        {{ index + 1 }}
                    </template>
                </v-data-table>
            </BCardBody>
            <BCardFooter>
               
            </BCardFooter>
        </BCard>
    </v-container>
</template>