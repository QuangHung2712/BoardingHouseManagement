<style scoped>
    .custom-table {
        height: 290px; /* Giới hạn chiều cao tối đa */
        overflow-y: auto; /* Hiển thị thanh cuộn dọc nếu vượt quá */
    }
    .custom-table thead tr {
        position: sticky !important; /* Giữ cố định header */
        top: 0; /* Cố định ở đỉnh */
        z-index: 1100; /* Đảm bảo header nằm trên nội dung */
        background-color: white; /* Màu nền cho header */
    }
</style>
<script>
    import pageheader from "@/components/page-header.vue"
    import Swal from "sweetalert2";

    export default {
        name: "HOMEPAGE",
        components: {
            pageheader
        },
        data(){
            return{
                viewdialog: false,
                titleDialog:'',
                form: false,
                headersTable:[
                    {title: 'Số phòng',value:'soPhong',sortable: true},
                    {title: 'Số tiền',value: 'SoTien',sortable: true},
                    {title: 'Ngày thanh toán', value: 'NgayThanhToan',sortable: true},
                    {title: '',value: 'actions',sortable: false}
                ],
                RequestPaymentConfirmation: [
                    {id: 1, soPhong: "102",SoTien: "3.000.000 VNĐ", NgayThanhToan: '20/12/2024'},
                    {id: 1, soPhong: "102",SoTien: "3.000.000 VNĐ", NgayThanhToan: '20/12/2024'},
                    {id: 1, soPhong: "102",SoTien: "3.000.000 VNĐ", NgayThanhToan: '20/12/2024'},

                ],
            }
        },
        methods:{
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
                    .then((result) => {
                        if (result.value) {
                            swalWithBootstrapButtons.fire(
                                "Xóa thành công!",
                                `Đã xóa phát sinh của phòng thành công: ${name}`,
                                "success"
                            );
                        } else if (
                            /* Read more about handling dismissals below */
                            result.dismiss === Swal.DismissReason.cancel
                        ) {
                            swalWithBootstrapButtons.fire(
                                "Xóa không thành công",
                                `Đã xảy ra lỗi khi xóa phát sinh của phòng: ${name}`,
                                "error"
                            );
                        }
                    });
            },
        }
}
</script>

<template>
    <pageheader title="" pageTitle="Trang chủ" />
    <BRow>
        <BCol class="col-sm-4">
            <BCard class="statistics-card-1" no-body>
                <BCardHeader class="d-flex align-items-center justify-content-between py-2">
                    <h5>Số phòng đã thanh toán</h5>
                </BCardHeader>
                <BCardBody class="p-3">
                    <div class="d-flex align-items-center">
                        <div class="avtar bg-brand-color-1 text-white me-3">
                            <i class="ph-duotone ph-currency-dollar f-26"></i>
                        </div>
                        <h3 class="f-w-300 d-flex align-items-center m-b-0">4</h3>
                    </div>
                    <p class="text-muted mb-2 text-sm mt-3">Các phòng đã thanh toán: 102, 103, 104</p>
                    <div class="progress" style="height: 7px">
                        <div class="progress-bar bg-brand-color-3" role="progressbar" style="width: 75%" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100"></div>
                    </div>
                </BCardBody>
            </BCard>
        </BCol>
        <BCol class="col-sm-4">
            <BCard class="statistics-card-1" no-body>
                <BCardHeader class="d-flex align-items-center justify-content-between py-2">
                    <h5>Số phòng chưa thanh toán</h5>
                </BCardHeader>
                <BCardBody class="p-3">
                    <div class="d-flex align-items-center">
                        <div class="avtar bg-brand-color-1 text-white me-3">
                            <i class="ph-duotone ph-currency-dollar f-26"></i>
                        </div>
                        <h3 class="f-w-300 d-flex align-items-center m-b-0">4</h3>
                    </div>
                    <p class="text-muted mb-2 text-sm mt-3">Các phòng chưa thanh toán: 202, 203, 204</p>
                    <div class="progress" style="height: 7px">
                        <div class="progress-bar bg-brand-color-3" role="progressbar" style="width: 75%" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100"></div>
                    </div>
                </BCardBody>
            </BCard>
        </BCol>
        <BCol class="col-sm-4">
            <BCard class="statistics-card-1" no-body>
                <BCardHeader class="d-flex align-items-center justify-content-between py-2">
                    <h5>Số phòng chưa có hoá đơn</h5>
                </BCardHeader>
                <BCardBody class="p-3">
                    <div class="d-flex align-items-center">
                        <div class="avtar bg-brand-color-1 text-white me-3">
                            <i class="ph-duotone ph-currency-dollar f-26"></i>
                        </div>
                        <h3 class="f-w-300 d-flex align-items-center m-b-0">4</h3>
                    </div>
                    <p class="text-muted mb-2 text-sm mt-3">Các phòng chưa có hoá đơn: 202, 203, 204</p>
                    <div class="progress" style="height: 7px">
                        <div class="progress-bar bg-brand-color-3" role="progressbar" style="width: 75%" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100"></div>
                    </div>
                </BCardBody>
            </BCard>
        </BCol>
        <BCol class="col-sm-4">
            <div class="card statistics-card-1">
                <div class="card-body">
                    <div class="d-flex align-items-center">
                        <div class="avtar bg-brand-color-1 text-white me-3">
                            <i class="ph-duotone ph-currency-dollar f-26"></i>
                        </div>
                        <div>
                            <p class="text-muted mb-0">Số phòng còn trống</p>
                            <div class="d-flex align-items-end">
                                <h2 class="mb-0 f-w-500">4</h2>
                                <span class="badge bg-light-danger ms-2">Phòng: 102, 103, 104</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card statistics-card-1">
                <div class="card-body">
                    <div class="d-flex align-items-center">
                        <div class="avtar bg-brand-color-1 text-white me-3">
                            <i class="ph-duotone ph-currency-dollar f-26"></i>
                        </div>
                        <div>
                            <p class="text-muted mb-0">Số phòng sắp hết hạn hợp đồng</p>
                            <div class="d-flex align-items-end">
                                <h2 class="mb-0 f-w-500">4</h2>
                                <span class="badge bg-light-warning ms-2">Phòng: 102, 103, 104</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card statistics-card-1">
                <div class="card-body">
                    <div class="d-flex align-items-center">
                        <div class="avtar bg-brand-color-1 text-white me-3">
                            <i class="ph-duotone ph-currency-dollar f-26"></i>
                        </div>
                        <div>
                            <p class="text-muted mb-0">Tổng số tiền đã thu được</p>
                            <div class="d-flex align-items-end">
                                <h2 class="mb-0 f-w-500">4 <small class="text-muted">VND</small></h2>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </BCol>
        <BCol class="col-sm-8">
            <BCard>
                <BCardHeader class="p-0">
                    <h4>Các phòng đang chờ xác nhận thanh toán</h4>
                </BCardHeader>
                <BCardBody class="p-0">
                    <v-data-table 
                        :headers = "headersTable"
                        :items="RequestPaymentConfirmation"
                        class="border-sm rounded-lg custom-table"
                        hide-default-footer>
                        <template v-slot:[`item.actions`]="{ item }">
                            <button class="btn btn-sm btn-link-danger" @click="Refuse(item.id)">Từ chối</button>
                            <button class="btn btn-sm btn-primary ms-1" @click="Received(item.id)">Đã nhận</button>
                        </template>
                    </v-data-table>
                </BCardBody>
            </BCard>
        </BCol>
    </BRow>
</template>