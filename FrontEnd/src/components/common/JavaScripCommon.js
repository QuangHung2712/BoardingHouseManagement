export function formatTablePrice(price) {
    // Chuyển số thành chuỗi và bỏ các ký tự không phải số
    let formattedPrice = price.toString().replace(/[^\d]/g, '');

    // Thêm dấu chấm sau mỗi 3 chữ số
    if (formattedPrice) {
        formattedPrice = formattedPrice.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
    }

    return formattedPrice + '  VND';
}
export function formatPrice(inputPrice) {
    // Loại bỏ tất cả các ký tự không phải là số
    let price = inputPrice.toString().replace(/[^\d]/g, '');
    // Chia chuỗi thành các nhóm ba chữ số và nối lại bằng dấu chấm
    if (price) {
        price = price.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
    }
    // Cập nhật giá trị cho price
    return price;
}
export default {
    formatTablePrice,
    formatPrice,
};