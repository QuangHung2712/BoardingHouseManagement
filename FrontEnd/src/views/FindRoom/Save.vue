<style scoped>
    p{
        font-size: 14px !important;
    }
    .v-window__left, .v-window__right {
        font-size: 16px; /* Điều chỉnh kích thước của nút */
        width: 10px !important; /* Thu nhỏ chiều rộng của nút */
        height: 30px; /* Thu nhỏ chiều cao của nút */
    }
</style>

<script>
    import apiClient from '@/plugins/axios';
    import store from "../../state/store";
    import common from "@/components/common/JavaScripCommon"


    export default {
        data(){
            return{
                window: 0,
                length: 3,
                roomSaveData:[],
                customerId: 0,
            }
        },
        created(){
            this.customerId = store.getters['getCustomerId'] ?? 0;
            this.GetSaveRoom();
        },
        methods:{
            GetSaveRoom(){
                apiClient.get(`/Customer/GetSaveRoom?id=${this.customerId}`)
                        .then(response=>{
                            this.roomSaveData = response.data;
                        })
                        .catch(error=>{
                            this.message = "Lấy danh sách phòng lưu bị lỗi " + error;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
            },
            SaveRoom(roomId){
                // Gửi yêu cầu API
                apiClient
                .put(`/Room/SaveRoom?customerId=${this.customerId}&roomId=${roomId}&status=${true}`)
                .then(() => {
                    // Hiển thị thông báo thành công
                    this.message = "Xoá phòng thành công";
                    this.snackbar = true;
                    this.snackbarColor = "green";
                    this.GetSaveRoom();
                })
                .catch((error) => {
                    // Xử lý lỗi
                    this.message =
                    "Lưu phòng đã xảy ra lỗi: " +
                    (error.response?.data?.message || error.message || "Không xác định");
                    this.snackbar = true;
                    this.snackbarColor = "red";
                })
            },
            GotoDetail(roomId){
                this.$router.push({ name: 'detail', params: { idroom: roomId } });
            },
            FormatPrice(price){
                return common.formatTablePrice(price);
            },
        }
    }
</script>
<template>
    <v-container>
        <v-row>
            <v-col cols="6">
                <BCard no-body>
                    <BCardHeader class="p-3">
                        <h4 class="m-0">Phòng đã lưu</h4>
                    </BCardHeader>
                    <BCardBody class="p-2">
                        <h2 v-if="roomSaveData == 0">Không có phòng nào</h2>
                        <BCard no-body v-else v-for="(room,index) in roomSaveData" :key="index">
                            <BRow>
                                <BCol class="col-md-4 col-12">
                                    <v-img
                                        src="/images/Room/20241119180012-04a6_wm.jpg"
                                        alt="Room Image"
                                        aspect-ratio="16/9"
                                        class="rounded elevation-2"
                                        contain
                                    ></v-img>
                                </BCol>
                                <BCol class="col-md-8 col-12 mt-2">
                                    <button @click="GotoDetail(room.roomId)" class="text-left">
                                        <h5> {{ room.towerName }} </h5>
                                        <h6>Giá: <span style="color: red;">{{ FormatPrice(room.price) }}</span></h6>
                                        <p><v-icon>mdi-map-marker-radius</v-icon>Địa chỉ: {{ room.towerAddress }} </p>
                                        <p>Thiết bị: {{ room.device }}</p>
                                    </button>
                                    <div class="text-right">
                                        <BButton variant="white" style="color: red;" @click="SaveRoom(room.roomId)"><v-icon>mdi-heart</v-icon></BButton>
                                    </div>
                                </BCol>
                            </BRow>
                        </BCard>
                    </BCardBody>
                </BCard>
            </v-col>
            <v-col cols="6">
                <BCard no-body>
                    <BCardHeader class="p-3">
                        <h4 class="m-0">Tin đã lưu</h4>
                    </BCardHeader>
                    <BCardBody class="p-2">
                       <h2>Không có tin nào</h2>
                    </BCardBody>
                </BCard>
                
            </v-col>
        </v-row>
    </v-container>
</template>