<style scoped>
    .table{
        text-align: center;
    }
</style>
<script>
    import common from "@/components/common/JavaScripCommon"
    import apiClient from '@/plugins/axios';

    export default {
        name: "Enter-Bill-Information",
        components: {
        },
        created(){
            const idBill = this.$route.params.idbill;
            this.getinfoBill(idBill);
        },
        data(){
            return{
                Status : false,
                email: 'Quanghungksdtqn@gmail.com',
                address: 'Số 11 Ngõ 91 Đường Cầu Diễn Phường Cầu Diễn Quận Nam Từ Liêm Thành phố Hà Nội',
                qrPay: '/eb67b8edadf110af49e0.jpg',
                BillData:{
                    id: 0,
                    numberOfRoom:'',
                    priceRoom: 0,
                    addressTower: '',
                    amount: 0,
                    date: '',
                    pathImgQRPay:'',
                    stk: '',
                    status: 0,
                    service: [
                        {
                            id: 0,
                            isOldNewNumber: false,
                            name: '',
                            newNumber: null,
                            oldNumber: null,
                            unitPrice: 0,
                            usageNumber: 1,
                        }
                    ],
                    arises:[
                        {
                            id: 0,
                            amount: 0,
                            reason: '',
                        }
                    ]
                },
                message: '',
                snackbar: false,
                snackbarColor: '',
                
            }
        },
        computed: {
            filteredServicesFalse() {
                return this.BillData.service.filter(item => !item.isOldNewNumber);
            },
            filteredServicesTrue() {
                return this.BillData.service.filter(item => item.isOldNewNumber);
            },
        },
        methods:{
            FormatPrice(price) {
                if(price)
                return common.formatTablePrice(price);
            },
            getinfoBill(idBill){
                apiClient.get(`/Bill/GetInfoBill/${idBill}`)
                        .then(response=>{
                            this.BillData = response.data;
                            console.log(response.data);
                        })
                        .catch(error=>{
                            this.message = "Lấy thông tin hoá đơn bị lỗi: " + error.response?.data?.message || error.message;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
            },
            btnCalculateInvoice(){
                apiClient.put(`/Bill/CalculateInvoice`,this.BillData)
                        .then(()=>{
                            this.message = "Tính tiền hoá đơn thành công. Đây là hoá đơn của bạn";
                            this.snackbar = true;
                            this.snackbarColor = 'green';
                            window.location.reload();
                        })
                        .catch(error=>{
                            this.message = "Tính tiền hoá đơn bị lỗi: " + error.response?.data?.message || error.message;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
            },
            PayBill(){
                apiClient.put(`/Bill/PayBill?biilId=${this.BillData.id}`)
                        .then(()=>{
                            this.message = "Thanh toán hoá đơn thành công. Bạn vui lòng chờ chủ nhà xác nhận thanh toán. Có gì chúng tôi sẽ liên hệ qua Email.";
                            this.snackbar = true;
                            this.snackbarColor = 'green';
                            window.location.reload();
                        })
                        .catch(error=>{
                            this.message = "Thanh toán hoá đơn bị lỗi: " + error.response?.data?.message || error.message;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
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
        <div class="d-flex justify-center" >
            <BCard  >
                <BCardHeader class="p-0 ">
                    <h3 class="px-lg-16 text-center">Nhập thông tin hoá đơn tháng {{ BillData.date }} của phòng {{ BillData.numberOfRoom }}</h3>
                    <p class="text-center"> <v-icon>mdi-map-marker</v-icon>Địa chỉ: {{ BillData.addressTower }}</p>
                </BCardHeader>
                <BCardBody class="p-0">
                    <BRow>
                        <table class="table Info" >
                            <thead>
                                <tr>
                                    <th style="width: 40%;">Tên dịch vụ</th>
                                    <th>Số tiền</th>
                                    <th>Sử dụng</th>
                                    <th>Thành tiền</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Tiền phòng</td>
                                    <td>{{ FormatPrice(BillData.priceRoom)}}</td>
                                    <td>1</td>
                                    <td>{{ FormatPrice(BillData.priceRoom)}}</td>
                                </tr>
                                <tr v-for="(serviceItem, index) in BillData.service" :key="index">
                                    <td>
                                        <div>Tiền {{ serviceItem.name }}</div> 
                                        <div v-show="serviceItem.newNumber != null">(Số cũ: {{ serviceItem.oldNumber }} - Số mới: {{ serviceItem.newNumber }})</div>
                                    </td>
                                    <td>{{ FormatPrice(serviceItem.unitPrice) }}</td>
                                    <td>{{ serviceItem.usageNumber }}</td>
                                    <td>{{ FormatPrice(serviceItem.unitPrice * serviceItem.usageNumber) }}</td>
                                </tr>
                                <tr v-for="(arise, index) in BillData.arises" :key="index">
                                    <td>Phát sinh({{ arise.reason }})</td>
                                    <td>{{ FormatPrice(arise.amount) }}</td>
                                    <td>1</td>
                                    <td>{{ FormatPrice(arise.amount) }}</td>
                                </tr>

                                <tr>
                                    <td colspan="3" v-show="BillData.status != 1 "><h4>Tổng tiền</h4></td>
                                    <td><h4>{{FormatPrice(BillData.amount) }}</h4></td>
                                </tr>
                            </tbody>
                        </table>
                        <BRow v-show="BillData.status == 2">
                            <BCol class="col-lg-6 d-flex justify-content-center align-items-center">
                                <div>
                                    <h4 class="text-center">Ngân hàng MB</h4>
                                    <h4 class="text-center">STK: {{ BillData.stk }}</h4>
                                    <h4 class="text-center">Chủ tài khoản: Phạm Quang Hưng</h4>
                                </div>
                            </BCol>
                            <BCol class="col-lg-6">
                                <h4 class="text-center">QR Ngân Hàng</h4>
                                <div class="d-flex justify-center">
                                    <img :src="BillData.pathImgQRPay" alt="Ảnh QR ngân hàng">
                                </div>
                            </BCol>
                        </BRow>
                        <BCol class="col-lg-6" v-for="(serviceItem, index) in filteredServicesTrue" :key="index" v-show="BillData.status == 1">
                            <div class="form-group m-0">
                                <label class="form-label">Nhập vào số {{ serviceItem.name }} (số cũ {{ serviceItem.oldNumber }}):</label>
                                <v-text-field type="number" v-model="serviceItem.newNumber" variant="outlined" :rules="[v => !!v || 'Trường này là bắt buộc']" clearable placeholder='Nhập số mới' class="input-control"></v-text-field>
                            </div>
                        </BCol>
                    </BRow>
                </BCardBody>
                <BCardFooter class="d-flex justify-center">
                    <v-btn @click="btnCalculateInvoice" v-show="BillData.status === 1">Tính hoá đơn</v-btn>
                    <v-btn @click="PayBill" v-show="BillData.status === 2">Thanh toán hoá đơn</v-btn>
                    <h3 v-show="BillData.status === 3" >Hoá đơn đã được thanh toán. Đang chờ chủ nhà xác nhận thanh toán</h3>
                </BCardFooter>
            </BCard>

        </div>
    </v-container>
    
</template>