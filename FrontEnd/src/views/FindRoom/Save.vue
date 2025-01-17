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
                postSaveData:[],
            }
        },
        created(){
            this.customerId = store.getters['getCustomerId'] ?? 0;
            this.GetSaveRoom();
            this.GetSavePost();
        },
        methods:{
            GetSaveRoom(){
                apiClient.get(`/Customer/GetSaveRoom?id=${this.customerId}`)
                        .then(response=>{
                            this.roomSaveData = response.data;
                            console.log(this.roomSaveData);
                        })
                        .catch(error=>{
                            this.message = "Lấy danh sách phòng lưu bị lỗi " + error;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
            },
            GetSavePost(){
                apiClient.get(`/Customer/GetSavePost?id=${this.customerId}`)
                        .then(response=>{
                            this.postSaveData = response.data;
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
                    "Xóa lưu phòng đã xảy ra lỗi: " +
                    (error.response?.data?.message || error.message || "Không xác định");
                    this.snackbar = true;
                    this.snackbarColor = "red";
                })
            },
            Savepost(postId){
                // Gửi yêu cầu API
                apiClient
                .put(`/Post/SavePost?customerId=${this.customerId}&postId=${postId}&status=${true}`)
                .then(() => {
                     // Hiển thị thông báo thành công
                    this.message = "Xoá phòng thành công";
                    this.snackbar = true;
                    this.snackbarColor = "green";
                    this.GetSavePost();
                })
                .catch((error) => {
                    // Xử lý lỗi
                    this.message =
                    "Xóa lưu bài đăng đã xảy ra lỗi: " +
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
                                <BCol class="col-md-4 col-12" >
                                    <v-window style="height: 100%;"
                                        v-model="room.selectimg"
                                        show-arrows
                                        theme="dark"
                                        v-if="room.img != 0"
                                    >
                                        <v-window-item
                                        v-for="(item,index) in room.img"
                                        :key="index"
                                        >
                                        <v-card class="d-flex justify-center align-center" height="200px">
                                            <v-img
                                                :src="item"
                                                alt="Room Image"
                                                aspect-ratio="16/9"
                                                class="rounded elevation-2"
                                                contain
                                            ></v-img>
                                        </v-card>
                                        </v-window-item>
                                    </v-window>
                                    <h4 v-else class="text-center">Không có ảnh</h4>
                                </BCol>
                                <BCol class="col-md-8 col-12 mt-2">
                                    <button @click="GotoDetail(room.roomId)" class="text-left">
                                        <h5>Phòng {{ room.towerName }} </h5>
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
                        <h4 class="m-0">Bài đăng đã lưu</h4>
                    </BCardHeader>
                    <BCardBody class="p-2">
                        <h2 v-if="postSaveData == 0">Không có bài đăng nào</h2>
                        <BCard no-body v-else v-for="(post,index) in postSaveData" :key="index">
                            <BRow>
                                <BCol class="col-md-4 col-12">
                                    <v-window style="height: 100%;"
                                        v-model="post.selectimg"
                                        show-arrows
                                        theme="dark"
                                        v-if="post.img != 0"
                                    >
                                        <v-window-item
                                        v-for="(item,index) in post.img"
                                        :key="index"
                                        >
                                        <v-card class="d-flex justify-center align-center" height="200px">
                                            <v-img
                                                :src="item"
                                                alt="Room Image"
                                                aspect-ratio="16/9"
                                                class="rounded elevation-2"
                                                contain
                                            ></v-img>
                                        </v-card>
                                        </v-window-item>
                                    </v-window>
                                    <h4 v-else class="text-center">Không có ảnh</h4>
                                </BCol>
                                <BCol class="col-md-8 col-12 mt-2">
                                    <button @click="GotoDetail(post.id)" class="text-left">
                                        <h5> {{ post.name }} </h5>
                                        <h6>Giá: <span style="color: red;">{{ FormatPrice(post.price) }}</span></h6>
                                        <p><v-icon>mdi-map-marker-radius</v-icon>Địa chỉ: {{ post.address }} </p>
                                        <p>Giới tính: {{ post.gender }}</p>
                                    </button>
                                    <div class="text-right">
                                        <BButton variant="white" style="color: red;" @click="Savepost(post.id)"><v-icon>mdi-heart</v-icon></BButton>
                                    </div>
                                </BCol>
                            </BRow>
                        </BCard>
                    </BCardBody>
                </BCard>
            </v-col>
        </v-row>
    </v-container>
</template>