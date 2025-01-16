<script>
    import pageheader from "@/components/page-header.vue"
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
                titleDialog:'',
                form: false,
                towerId: 0,
                message: '',
                snackbar: false,
                snackbarColor: '',
                searchStratDate: null,
                searchEndDate: null,
                ElectricChat: [
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
                            text: 'Doanh thu',
                        },
                    },
                    dataLabels: {
                        enabled: false,
                    },
                    colors: ['#4285F4'], // Màu cột
                },
                chartSeries: [
                    {
                        name: 'Doanh thu',
                        data: [],
                    },
                ],
            }
        },
        created(){
            const idtower = this.$route.params.idtower;
            const DecodingIdTower = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Base64.parse(idtower));
            this.towerId = DecodingIdTower;
        },
        methods:{
            initializeChart() {
                // Gán dữ liệu cho biểu đồ
                this.chartOptions.xaxis.categories = this.ElectricChat.map(item=>item.month);
                this.chartSeries[0].data = this.ElectricChat.map(item=>item.quantity);
            },
            ViewReport(){
                const [startYear, startMonth] = this.searchStratDate.split("-");
                const StratDate = `${startYear}-${startMonth}-01`;
                const [endYear, endMonth] = this.searchEndDate.split("-");
                const lastDayOfMonth = new Date(endYear, endMonth, 0).getDate();
                const EndDate = `${endYear}-${endMonth}-${lastDayOfMonth}`;
                apiClient.get(`/Tower/ReportByTower`,{
                    params: {
                        towerId: this.towerId,
                        stratDate: StratDate,
                        endDate: EndDate,
                    },
                })
                .then(response=>{
                    this.ElectricChat = response.data;
                    this.chartOptions.xaxis.categories = response.data.map(item=>item.month);
                    this.chartSeries[0].data= response.data.map(item=>item.quantity);
                })
                .catch(error=>{
                    this.message = "Báo cáo bị lỗi: " + error.response?.data?.message || error;
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                })
            }
        },
    }
</script>

<template>
    <pageheader title="" pageTitle="Báo cáo" />
    <v-snackbar
        v-model="snackbar"
        :timeout="10000"
        class="custom-snackbar"
        :color="snackbarColor"
    >
        <h5 class="text-center">{{ message }}</h5>
    </v-snackbar>
    <BCard>
        <BCardBody class="p-0">
            <BRow>
                <BCol class="col-sm-4">
                    <div class="form-group m-0">
                        <label class="form-label m-0">Từ ngày:</label>
                        <input type="month" class="form-control" id="example-datemin" v-model="searchStratDate" min="2000-01-02" placeholder="Tìm kiếm từ ngày">
                    </div>
                </BCol>
                <BCol class="col-sm-4">
                    <div class="form-group m-0">
                        <label class="form-label m-0">Từ ngày:</label>
                        <input type="month" class="form-control" id="example-datemin" v-model="searchEndDate" min="2000-01-02" placeholder="Tìm kiếm từ ngày">
                    </div>
                </BCol>
                <BCol class="col-sm-4 d-flex align-end justify-center">
                    <v-btn color="blue-lighten-1" class="mt-2" @click="ViewReport()">Xem báo cáo</v-btn>
                </BCol>
            </BRow>
        </BCardBody>
    </BCard>
    <BCard>
        <BCardHeader class="p-2">
            <h4>Doanh thu</h4>
        </BCardHeader>
        <BCardBody class="p-0">
            <div class="overview-product-legends text-center">
                <p class="text-muted mb-1"><span>Tổng doanh thu</span></p>
                <h4 class="mb-0">{{ chartSeries[0].data.reduce((a, b) => a + b, 0) }}</h4>
            </div>
            <apexchart type="bar" :options="chartOptions" :series="chartSeries"></apexchart>
        </BCardBody>
    </BCard>
</template>