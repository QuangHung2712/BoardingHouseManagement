<style scoped>
    .table{
        text-align: center;
    }
</style>
<script>
    import CryptoJS from "crypto-js";
    const key = CryptoJS.enc.Utf8.parse("kkFVQVMYIdJglUxs"); // Trùng với key backend
    const iv = CryptoJS.enc.Utf8.parse("dLnolzxmIyvmcfSD"); // Trùng với IV backend
    export default {
        name: "Enter-Bill-Information",
        components: {
        },
        created(){
            const idBill = this.$route.params.idbill;
            console.log(this.Decrypt(idBill));
        },
        data(){
            return{
                numberOfRoom: 102,
                Status : false,
                email: 'Quanghungksdtqn@gmail.com',
                address: 'Số 11 Ngõ 91 Đường Cầu Diễn Phường Cầu Diễn Quận Nam Từ Liêm Thành phố Hà Nội',
                qrPay: '/eb67b8edadf110af49e0.jpg',
                
            }
        },
        methods:{
            Decrypt(cipherText) {
                const bytes = CryptoJS.AES.decrypt(cipherText, key, {
                    iv: iv,
                    mode: CryptoJS.mode.CBC,
                    padding: CryptoJS.pad.Pkcs7,
                });
                return bytes.toString(CryptoJS.enc.Utf8);
            }
        }
    }
</script>
<template>
    <v-container>
        <div class="d-flex justify-center" >
            <BCard v-if="!Status" >
                <BCardHeader class="p-0 p-md-3">
                    <h3 class="px-lg-16 text-center">Nhập thông tin hoá đơn tháng 12 của phòng {{ numberOfRoom }}</h3>
                </BCardHeader>
                <BCardBody>
                    <BRow>
                        <BCol class="col-lg-6">
                            <div class="form-group">
                                <label class="form-label">Nhập vào số điện:</label>
                                <v-text-field type="number" variant="outlined" clearable placeholder="Nhập số điện mới" class="input-control"></v-text-field>
                            </div>
                        </BCol>
                        <BCol class="col-lg-6">
                            <div class="form-group">
                                <label class="form-label">Nhập vào số nước:</label>
                                <v-text-field type="number" variant="outlined" clearable placeholder="Nhập số nước mới" class="input-control"></v-text-field>
                            </div>
                        </BCol>
                    </BRow>
                </BCardBody>
                <BCardFooter class="d-flex justify-center">
                    <v-btn>Tính hoá đơn</v-btn>
                </BCardFooter>
            </BCard>
            <BCard v-else>
                <BCardHeader class="p-0">
                    <h3 class="text-center">Thông tin hoá đơn tháng 12 của phòng {{ numberOfRoom }}</h3>
                    <p class="text-center"> <v-icon>mdi-map-marker</v-icon>Địa chỉ: {{ address }}</p>
                </BCardHeader>
                <table class="table Info">
                    <thead>
                        <tr>
                            <th>STT</th>
                            <th style="width: 40%;">Tên dịch vụ</th>
                            <th>Số tiền</th>
                            <th>Sử dụng</th>
                            <th>Thành tiền</th>
                        </tr>
                    </thead>
                    <tbody>
                            <tr>
                                <td>1</td>
                                <td>Tiền phòng</td>
                                <td>3.000.000 VNĐ</td>
                                <td>1</td>
                                <td>3.000.000 VNĐ</td>
                            </tr>
                            <tr>
                                <td>2</td>
                                <td>
                                    <div>Tiền điện</div> 
                                    <div>(Số cũ: 100 - Số mới: 1000)</div>
                                </td>
                                <td>3.000 VNĐ</td>
                                <td>1</td>
                                <td>3.000.000 VNĐ</td>
                            </tr>
                            <tr>
                                <td>3</td>
                                <td>Tiền nước</td>
                                <td>30.000 VNĐ</td>
                                <td>1</td>
                                <td>3.000.000 VNĐ</td>
                            </tr>
                            <tr>
                                <td>4</td>
                                <td>Tiền phòng</td>
                                <td>3.000.000 VNĐ</td>
                                <td>1</td>
                                <td>3.000.000 VNĐ</td>
                            </tr>
                    </tbody>
                </table>
                <BCardBody>
                    <BRow>
                        <BCol class="col-lg-6">
                            <h4 class="text-center">Ngân hàng MB</h4>
                            <h4 class="text-center">STK: 0080127122002</h4>
                            <h4 class="text-center">Chủ tài khoản: Phạm Quang Hưng</h4>
                        </BCol>
                        <BCol class="col-lg-6">
                            <h4 class="text-center">QR Ngân Hàng</h4>
                            <div class="d-flex justify-center">
                                <img :src="qrPay" alt="">
                            </div>
                        </BCol>
                    </BRow>
                </BCardBody>
                <BCardFooter class="d-flex justify-center">
                    <v-btn>Đã Thanh toán</v-btn>
                </BCardFooter>
            </BCard>
        </div>
    </v-container>
    
</template>