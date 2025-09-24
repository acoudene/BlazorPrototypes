window.qrScanner = {
  start: function (dotNetHelper, elementId) {
    const html5QrCode = new Html5Qrcode(elementId);

    html5QrCode.start(
      { facingMode: "environment" },
      { fps: 10, qrbox: 250 },
      (decodedText, decodedResult) => {
        dotNetHelper.invokeMethodAsync("OnQrCodeScanned", decodedText);
      },
      (errorMessage) => {
        console.log("QR Scan error:", errorMessage);
      }
    ).catch(err => {
      console.error("Camera start failed:", err);
    });

    window.qrScanner._instance = html5QrCode;
  },
  stop: function () {
    if (window.qrScanner._instance) {
      window.qrScanner._instance.stop().then(() => {
        console.log("Scanner stopped");
      });
    }
  }
};
