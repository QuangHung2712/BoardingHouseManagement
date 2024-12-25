<script>
    import pageheader from "@/components/page-header.vue"

    export default {
        name: "PRODUCT-LIST",
        components: {
            pageheader
        },
        data(){
            return{
                viewdialog: false,
                titleDialog:'',
                form: false,
                ElectricChat: [
                    {thang: 1,data: 10},
                    {thang: 2,data: 13},
                    {thang: 3,data: 15},
                    {thang: 4,data: 18},
                    {thang: 5,data: 20},
                    {thang: 6,data: 16},
                ],
                chartOptions: {
                    chart: {
                    type: 'bar',
                    height: 350,
                    },
                    plotOptions: {
                    bar: {
                        horizontal: false,
                        columnWidth: '55%',
                    },
                    },
                    xaxis: {
                    categories: [],
                    title: {
                        text: 'Tháng',
                    },
                    },
                    yaxis: {
                    title: {
                        text: 'Số Điện',
                    },
                    },
                    dataLabels: {
                    enabled: false,
                    },
                    colors: ['#4285F4'], // Màu cột
                },
                chartSeries: [
                    {
                    name: 'Số Điện',
                    data: [],
                    },
                ],
            }
        },
        mounted() {
            this.initializeChart();
        },
        methods:{
            initializeChart() {
                // Gán dữ liệu cho biểu đồ
                this.chartOptions.xaxis.categories = this.ElectricChat.map((item) => `Tháng ${item.thang}`);
                this.chartSeries[0].data = this.ElectricChat.map((item) => item.data);
            },
        },
    }
</script>

<template>
    <pageheader title="" pageTitle="Báo cáo" />
    <BCard>
        <BCardBody class="p-0">
            <BRow>
                <BCol class="col-sm-4">
                    <div class="form-group m-0">
                        <label class="form-label m-0">Từ ngày:</label>
                        <input type="month" class="form-control" id="example-datemin" v-model="searchRentalDate" min="2000-01-02" placeholder="Tìm kiếm từ ngày">
                    </div>
                </BCol>
                <BCol class="col-sm-4">
                    <div class="form-group m-0">
                        <label class="form-label m-0">Từ ngày:</label>
                        <input type="month" class="form-control" id="example-datemin" v-model="searchRentalDate" min="2000-01-02" placeholder="Tìm kiếm từ ngày">
                    </div>
                </BCol>
                <BCol class="col-sm-4 d-flex align-end justify-center">
                    <v-btn color="blue-lighten-1" class="mt-2" @click="(viewdialogEdit = !viewdialogEdit) && (EditRoom(0,'Thêm dịch vụ'))">Xem báo cáo</v-btn>
                </BCol>
            </BRow>
        </BCardBody>
    </BCard>
    <BCard>
        <BCardHeader class="p-2">
            <h4>Số điện</h4>
        </BCardHeader>
        <BCardBody class="p-0">
            <div class="overview-product-legends text-center">
                <p class="text-muted mb-1"><span>Tổng số điện</span></p>
                <h4 class="mb-0">250</h4>
            </div>
            <apexchart type="bar" :options="chartOptions" :series="chartSeries"></apexchart>
        </BCardBody>
    </BCard>
</template>