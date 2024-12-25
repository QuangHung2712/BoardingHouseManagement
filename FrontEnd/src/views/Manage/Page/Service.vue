<script>
import pageheader from "@/components/page-header.vue"
import Swal from "sweetalert2";
import apiClient from "@/plugins/axios";
import CryptoJS from 'crypto-js';
export default {
    name: "PRODUCT-LIST",
    components: {
        pageheader
    },
    data(){
        return{
            viewdialog: false,
            snackbar: false,
            message: 'Đây là một thông báo!',
            snackbarColor: 'red',
            titleDialog:'',
            searchName: '',
            towerId: 0,
            form: false,
            headersTable:[
                    {title: 'STT', value: 'stt',sortable: true},
                    {title: 'Tên dịch vụ',value:'name',sortable: true},
                    {title: 'Giá',value: 'price',sortable: true},
                    {title: 'Hành đồng',value: 'actions',sortable: false}
                ],
            serviceData: [

            ],
            selectService : { },
            dropdown: [],
        }
    },
    computed:{
        filterService(){
            return this.serviceData.filter(service =>service.name.toLowerCase().includes((this.searchName || '').toLowerCase()))
        }
    },
    created(){
        const idtower = this.$route.params.idtower;
        const DecodingIdTower = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Base64.parse(idtower));
        this.towerId = DecodingIdTower;
        this.GetAllServiceByTower();
    },
    methods:{
        async GetAllServiceByTower(){
            await apiClient.get(`/Service/GetAll?towerId=${this.towerId}`)
            .then(reponse =>{
                this.serviceData = reponse.data;
            })
            .catch(error =>{
                console.error('Lấy thông tin của dịch vụ: ',error);
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
                    text: `Bạn đang muốn xóa dịch vụ: ${name}`,
                    icon: "warning",
                    confirmButtonText: "Có!",
                    cancelButtonText: "Không!",
                    showCancelButton: true,
                })
                .then((confirm) => {
                    if (confirm.value) {
                            apiClient.delete(`/Service/DeleteService/${id}`)
                                    .then(reponse=> {
                                        console.log(reponse)
                                        if(reponse.status){
                                            swalWithBootstrapButtons.fire(
                                            "Xóa thành công!",
                                            `Đã xóa thành công dịch vụ: ${name}`,
                                            "success")
                                            this.GetAllServiceByTower()
                                        }
                                        else{
                                            swalWithBootstrapButtons.fire(
                                                "Xóa không thành công",
                                                `Đã xảy ra lỗi khi xóa dịch vụ: ${name}`,
                                                "error"
                                            );
                                        }
                                    })
                                    .catch(error =>{
                                        swalWithBootstrapButtons.fire(
                                            "Xóa không thành công",
                                            ` ${error}`,
                                            "error"
                                        );
                                    })
                    } else if ( /* Read more about handling dismissals below */ confirm.dismiss === Swal.DismissReason.cancel) return
                });
        },
        DetailService(serviceId,title){
            this.titleDialog = title;
            if(serviceId===0){
                this.selectService.name = "";
                this.selectService.price = "";
                this.form = false;
            }
            else{
                const service = this.serviceData.find(service => service.id === serviceId);
                this.selectService = {...service}
            }
        },
        required (v) {
            return !!v || 'Vui lòng không để trống'
        },
        requiredNumber (v) {
            return !!v || 'Vui lòng nhập vào số'
        },
        CreateEditService(){
            this.$nextTick(() => {
                this.$refs.form.validate(); // Gọi validate ngay sau khi modal được render
            });
            this.selectService.towerId = this.towerId;
            if(this.selectService.id=== undefined ){
                this.message = 'Thêm thành công'
                this.selectService.id = 0;
                this.selectService.applyPriceServiceAllRoom = false;
            }
            else{
                this.message = 'Sửa thành công'
            }
            if(!/^\d+$/.test(this.selectService.price))
                this.selectService.price =  this.selectService.price.replace(/[^\d]/g, '');
            apiClient.post(`/Service/CreateEditService`,this.selectService)
                .then(response => {
                    if(response.status){
                        this.snackbarColor = 'green'
                        this.snackbar = true
                        this.viewdialog = false
                        this.GetAllServiceByTower();
                        console.log(this.selectService)
                    }
                    else{
                        this.message = 'Đã xảy ra lỗi '
                        this.snackbarColor = 'red'
                        this.snackbar = true
                    }
                })
                .catch(error =>{
                    this.message = 'Đã xảy ra lỗi ' + error
                    this.snackbarColor = 'red'
                    this.snackbar = true
                })
        },
        formatPrice() {
            // Loại bỏ tất cả các ký tự không phải là số
            let price = this.selectService.price.replace(/[^\d]/g, '');
            // Chia chuỗi thành các nhóm ba chữ số và nối lại bằng dấu chấm
            if (price) {
                price = price.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            }
            // Cập nhật giá trị cho price
            this.selectService.price = price;
        },
        formatTablePrice(price) {
            // Chuyển số thành chuỗi và bỏ các ký tự không phải số
            let formattedPrice = price.toString().replace(/[^\d]/g, '');

            // Thêm dấu chấm sau mỗi 3 chữ số
            if (formattedPrice) {
                formattedPrice = formattedPrice.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            }

            return formattedPrice + '  VND';
        },
    }
}
</script>



<template>
        <pageheader title="" pageTitle="Dịch vụ" />
        <BRow>
            <BCol class="col-sm-12">
                <v-snackbar
                    v-model="snackbar"
                    :timeout="10000"
                    class="custom-snackbar"
                    :color="snackbarColor"
                >
                    <h4 class="text-center">{{ message }}</h4>
                </v-snackbar>
                <BCard no-body class="table-card p-sm-2">
                    <BCardBody>
                        <BRow class="text-end p-sm-2 ">
                            <BCol class="col-sm-2 d-sm-block d-none"></BCol>
                            <BCol class="col-sm-4 col-6"><v-text-field label="Tên dịch vụ" variant="outlined" v-model="searchName" clearable hide-details></v-text-field></BCol>
                            <BCol class="col-sm-2 col-6"><v-btn @click="(viewdialog = !viewdialog) && (DetailService(0,'Thêm dịch vụ'))" color="blue-lighten-1" class="mt-2"> Thêm dịch vụ </v-btn></BCol>
                            <BCol class="col-sm-2 col-6"><v-btn color="blue-lighten-1" class="mt-2" > Xuất ra Excel </v-btn></BCol>
                            <BCol class="col-sm-2 col-6"><v-btn color="blue-lighten-1" class="mt-2"> Nhập từ Excel </v-btn></BCol>
                        </BRow>
                        
                        <v-data-table 
                            :headers = "headersTable"
                            :items="filterService"
                            class="border-sm rounded-lg">
                            <template v-slot:[`item.stt`]="{ index }">
                                {{ index + 1 }}
                            </template>
                            <template v-slot:[`item.price`]="{ item }">
                                <!-- Hiển thị giá đã định dạng -->
                                {{ formatTablePrice(item.price) }}
                            </template>
                            <template v-slot:[`item.actions`]="{ item }">
                                <v-icon class="ml-5" small @click="(viewdialog = !viewdialog) && (DetailService(item.id,'Sửa dịch vụ'))" >mdi-pencil-circle </v-icon>
                                <v-icon class="ml-5" small @click="deleteService(item.id,item.name)" >mdi-delete-empty </v-icon>
                            </template>
                        </v-data-table>
                    </BCardBody>
                </BCard>
                <BModal v-model="viewdialog" hide-footer :title="titleDialog" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="lg" >
                    <div class="card-body">
                        <v-form v-model="form" ref="form">
                            <BRow>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Tên dịch vụ:</label>
                                        <!-- <input type="email" class="form-control" placeholder="Nhập vào tên dịch vụ"> -->
                                        <v-text-field v-model="selectService.name" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào tên dịch vụ" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Giá:</label>
                                        <!-- <input type="email" class="form-control" placeholder="Nhập vào giá của dịch vụ"> -->
                                        <v-text-field v-model="selectService.price" :rules="[requiredNumber]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào tên dịch vụ" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <v-switch
                                    v-show="selectService.id !== undefined"
                                    v-model="selectService.applyPriceServiceAllRoom"
                                    :label= "`Bạn có muốn áp dụng giá  ${selectService.price} cho tất cả các phòng đang dùng dịch vụ này không`" 
                                    color="primary"
                                    hide-details
                                    :style="{ color: 'blue' }" 
                                ></v-switch>
                            </BRow>
                        </v-form>
                    </div>
                    <div class="modal-footer v-modal-footer">
                        <BButton type="button" variant="light" @click="viewdialog = false">Close
                        </BButton>
                        <BButton type="button" variant="primary" @click="CreateEditService()" :disabled="!form">Save Changes</BButton>
                    </div>
                </BModal>
            </BCol>
        </BRow>
</template>
<style scoped>
    .custom-snackbar {
        top: 15vh!important;
        left: 70%;
        bottom: auto !important; /* Đảm bảo không hiển thị ở dưới */
    }
</style>