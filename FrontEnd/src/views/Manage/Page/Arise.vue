<script>
import pageheader from "@/components/page-header.vue"
import apiClient from "@/plugins/axios";
import Swal from "sweetalert2";
import CryptoJS from 'crypto-js';
import common from "@/components/common/JavaScripCommon"


export default {
    name: "HOME PAGE",
    components: {
        pageheader
    },
    data(){
        return{
            viewdialog: false,
            titleDialog:'',
            form: false,
            towerId: 0,
            headersTable:[
                    {title: 'STT', value: 'stt',sortable: true},
                    {title: 'Số phòng',value:'roomName',sortable: true},
                    {title: 'Số tiền',value: 'amount',sortable: true},
                    {title: 'Ngày tạo',value: 'creationDate',sortable: true},
                    {title: 'Lý do',value: 'reason',sortable: true},
                    {title: 'Trạng thái thanh toán',value: 'statusPay',sortable: true},
                    {title: 'Hành đồng',value: 'actions',sortable: false}
                ],
            AriseData: [

            ],
            message: '',
            snackbar: false,
            snackbarColor: '',
            arise:{},
            roomData: [],
            searchCreationDate: '',
            searchRoom: null,
            searchStatusPay: null,
        }
    },

    created(){
        const idtower = this.$route.params.idtower;
        const DecodingIdTower = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Base64.parse(idtower));
        this.towerId = DecodingIdTower;
        this.GetAllArise();
        this.getDropDownRoom();
    },
    computed:{
        filteredArise(){
            return this.AriseData.filter((a) => {
                const matchesSearchRoomName = this.searchRoom
                    ? a.roomName == this.searchRoom 
                    : true;

                // Lọc theo ngày thuê
                const matchesSearchCreationDate = this.searchCreationDate
                    ? new Date(a.creationDate).toISOString().split('T')[0] === this.searchCreationDate
                    : true;

                // Lọc theo tên khách hàng
                const matchesSearchStatusPay = this.searchStatusPay !== undefined && this.searchStatusPay !== null
                    ? (a.statusPay == this.searchStatusPay)
                    : true;
                // Kết hợp tất cả các điều kiện
                return matchesSearchRoomName && matchesSearchCreationDate && matchesSearchStatusPay;

            });
        }
    },
    methods:{
        GetAllArise(){
            apiClient.get(`/Incur/GetAllIncur/${this.towerId}`)
                    .then(response=>{
                        this.AriseData = response.data;
                    })
                    .catch(error=>{
                        this.message = "Lấy thông tin của phát sinh bị lỗi " + error;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })

        },
        deleteService(id,name) {
            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: "btn btn-success",
                    cancelButton: "btn btn-danger ml-2",
                },
                buttonsStyling: false,
            });

            swalWithBootstrapButtons
                .fire({
                    title: "Bạn có chắc chắn không?",
                    text: `Bạn đang muốn xóa phát sinh của phòng: ${name}`,
                    icon: "warning",
                    confirmButtonText: "Có!",
                    cancelButtonText: "Không!",
                    showCancelButton: true,
                })
                .then((confirm) => {
                    if (confirm.value) {
                            apiClient.delete(`/Incur/DeleteIncur/${id}`)
                                    .then(reponse=> {
                                        console.log(reponse)
                                        if(reponse.status){
                                            swalWithBootstrapButtons.fire(
                                            "Xóa thành công!",
                                            `Đã xóa phát sinh của phòng: ${name} thành công`,
                                            "success")
                                            this.GetAllArise();
                                        }
                                        else{
                                            swalWithBootstrapButtons.fire(
                                                "Xóa không thành công",
                                                `Đã xảy ra lỗi khi xóa phát sinh của phòng: ${name}`,
                                                "error"
                                            );
                                        }
                                    })
                                    .catch(error =>{
                                        swalWithBootstrapButtons.fire(
                                            "Xóa không thành công",
                                            ` ${error.response.data.message}`,
                                            "error"
                                        );
                                    })
                    } else if ( /* Read more about handling dismissals below */ confirm.dismiss === Swal.DismissReason.cancel) return
                });
        },
        async getDropDownRoom(){
            await apiClient.get(`/Room/GetDropDown?towerId=${this.towerId}`)
                            .then(response=>{
                                this.roomData = response.data;
                            })
                            .catch(error=>{
                                this.message = "Lấy thông tin của hợp đồng bị lỗi " + error;
                                this.snackbar = true;
                                this.snackbarColor = 'red';
                            });
        },
        ViewEdit(id,title){
            this.titleDialog = title;
            this.arise.id = id
            if(id===0){
                this.arise.roomId =null;
                this.arise.amount =""; 
                this.arise.reason =""; 
                this.form = false;
            }
            else{
                const result = this.AriseData.find(a => a.id === id);
                this.arise = {...result}
            }
        },
        required (v) {
            return !!v || 'Vui lòng không để trống'
        },
        requiredNumber (v) {
            return !!v || 'Vui lòng nhập vào số'
        },
        FormatPrice(){
            this.arise.amount = common.formatPrice(this.arise.amount);
        },
        FormatTablePrice(price) {
            return common.formatTablePrice(price);
        },
        formatDate(dateString) {
            const date = new Date(dateString);
            return date.toLocaleDateString('vi-VN'); // Định dạng: dd/MM/yyyy
        },
        CreateEditArise(){
            this.arise.towerId = this.towerId;
            if(this.arise.id ===0){
                this.message = "Thêm "
            }
            else{
                this.message = "Sửa "
            }
            if(!/^\d+$/.test(this.arise.amount))
                this.arise.amount = this.arise.amount.replace(/[^\d]/g, '');
            apiClient.post(`/Incur/CreateEditIncur`,this.arise)
                    .then(()=>{
                        this.message += "thành công"
                        this.snackbar = true;
                        this.snackbarColor = 'green';
                        this.viewdialog = false;
                        this.GetAllArise();
                    })
                    .catch(error =>{
                        this.message += "bị lỗi: "+ error.response.data.message;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
        },
        getStatusClass(status) {
            switch (status) {
                case true:
                    return 'badge bg-light-success ms-2'; // class dành cho trạng thái Active
                case false:
                    return 'badge bg-light-danger ms-2'; // class dành cho trạng thái Inactive
                default:
                    return 'badge bg-light-warning'; // class mặc định
            }
        },
    }
}
</script>

<style scoped>

</style>

<template>
        <pageheader title="" pageTitle="Phát sinh" />
        <BRow>
            <BCol class="col-sm-12">
                <v-snackbar
                    v-model="snackbar"
                    :timeout="10000"
                    class="custom-snackbar"
                    :color="snackbarColor"
                >
                    <h5 class="text-center">{{ message }}</h5>
                </v-snackbar>
                <BCard no-body class="table-card p-sm-2">
                    <BCardBody>
                        <BRow class="text-end p-sm-2 ">
                            <BCol class="col-sm-2 col-6">
                                <v-select 
                                    :items="roomData" 
                                    item-title="name"
                                    item-value="name" 
                                    v-model="searchRoom"
                                    hide-details 
                                    label="Số phòng" 
                                    
                                    clearable></v-select>
                            </BCol>
                            <BCol class="col-sm-3 col-6"><input type="date" class="form-control" id="example-datemin" v-model="searchCreationDate" min="2000-01-02"></BCol>
                            <BCol class="col-sm-3 col-6">
                                <v-select label="Trạng thái phí" 
                                    hide-details 
                                    :items="[ {title: 'Chưa thanh toán',value: false},
                                        {title: 'Đã thanh toán',value: true},]" 
                                    clearable
                                    v-model="searchStatusPay"
                                    ></v-select>
                            </BCol>
                            <BCol class="col-sm-2 col-6"><v-btn @click="(viewdialog = !viewdialog) && (ViewEdit(0,'Thêm phát sinh'))" color="blue-lighten-1" class="mt-2"> Thêm phát sinh </v-btn></BCol>
                            <BCol class="col-sm-2 col-6"><v-btn color="blue-lighten-1" class="mt-2" > Xuất ra Excel </v-btn></BCol>
                        </BRow>
                        <v-data-table 
                            :headers = "headersTable"
                            :items="filteredArise"
                            class="border-sm rounded-lg">
                            <template v-slot:[`item.stt`]="{ index }">
                                {{ index + 1 }}
                            </template>
                            <template v-slot:[`item.amount`]="{ item }">
                                <!-- Hiển thị giá đã định dạng -->
                                {{ FormatTablePrice(item.amount) }}
                            </template>
                            <template v-slot:[`item.creationDate`]="{ item }">
                                <!-- Hiển thị giá đã định dạng -->
                                {{ formatDate(item.creationDate) }}
                            </template>
                            <template v-slot:[`item.statusPay`]="{ item }">
                                <span :class="getStatusClass(item.statusPay)">
                                    {{ item.statusPay ? 'Thanh toán' : 'Chưa thanh toán' }}
                                </span>
                            </template>
                            <template v-slot:[`item.actions`]="{ item }">
                                <v-icon class="ml-lg-3" small @click="(viewdialog = !viewdialog) && (ViewEdit(item.id,'Sửa phát sinh'))" >mdi-pencil-circle </v-icon>
                                <v-icon class="ml-lg-3" small @click="deleteService(item.id,item.roomName)" >mdi-delete-empty </v-icon>
                            </template>
                        </v-data-table>
                    </BCardBody>
                </BCard>
                <BModal v-model="viewdialog" hide-footer :title="titleDialog" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="lg" >
                    <div class="card-body">
                        <v-form v-model="form">
                            <BRow>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Số phòng:</label>
                                        <v-select 
                                            :items="roomData" 
                                            item-title="name"
                                            item-value="id" 
                                            v-model="arise.roomId" 
                                            placeholder="Chọn số phòng"
                                            :readonly="arise.id != 0"
                                            :clearable="arise.id == 0">
                                        </v-select>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Số tiền:</label>
                                        <v-text-field v-model="arise.amount" :rules="[requiredNumber]" variant="outlined" type="text" @input="FormatPrice" clearable placeholder="Nhập vào số tiền" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-12">
                                    <div class="form-group">
                                        <label class="form-label">Lý do:</label>
                                        <v-text-field v-model="arise.reason" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào lý do" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                            </BRow>
                        </v-form>
                    </div>
                    <div class="modal-footer v-modal-footer">
                        <BButton type="button" variant="light" @click="viewdialog = false">Close
                        </BButton>
                        <BButton type="button" variant="primary" @click="CreateEditArise()" :disabled="!form">Save Changes</BButton>
                    </div>
                </BModal>
            </BCol>
        </BRow>
</template>