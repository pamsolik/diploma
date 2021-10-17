window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    QRCode(document.getElementById("qrCode"),
        {
            text: uri,
            width: 300,
            height: 300
        });
});