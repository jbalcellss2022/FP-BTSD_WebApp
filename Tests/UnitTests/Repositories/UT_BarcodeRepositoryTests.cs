using DataAccessLayer.Interfaces;
using Entities.Models;
using Moq;

namespace Tests.UnitTests.Repositories
{
    [TestFixture()]
    public class UT_BarcodeRepositoryTests
    {
        private Mock<IBarcodeRepository> _mockBarcodeRepository;

        [SetUp]
        public void SetUp()
        {
            _mockBarcodeRepository = new Mock<IBarcodeRepository>();
        }

        /// <summary>
        /// Tests the GetCBStaticProducts method to verify it returns the correct paginated list of static barcodes.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBStaticProducts method returns a paginated list of static barcodes when provided with a valid userGuid, pageIndex, and pageSize.
        /// </remarks>
        [Test()]
        public async Task GetCBStaticProductsTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var pageIndex = 1;
            var pageSize = 10;
            var expectedProducts = new PaginatedStaticBarcodeList<AppCBStatic>
            {
                Items = new List<AppCBStatic>
                {
                    new AppCBStatic { Id = Guid.NewGuid(), Code = "1234567890123" },
                    new AppCBStatic { Id = Guid.NewGuid(), Code = "9876543210987" }
                },
                PageIndex = pageIndex,
                TotalCount = 2
            };
            _mockBarcodeRepository.Setup(repo => repo.GetCBStaticProducts(userGuid, pageIndex, pageSize))
                .ReturnsAsync(expectedProducts);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.GetCBStaticProducts(userGuid, pageIndex, pageSize);

            // Assert
            Assert.That(result, Is.EqualTo(expectedProducts));
        }

        /// <summary>
        /// Tests the GetCBDynamicProducts method to verify it returns the correct paginated list of dynamic barcodes.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBDynamicProducts method returns a paginated list of dynamic barcodes when provided with a valid userGuid, pageIndex, and pageSize.
        /// </remarks>
        [Test()]
        public async Task GetCBDynamicProductsTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var pageIndex = 1;
            var pageSize = 10;
            var expectedProducts = new PaginatedDynamicBarcodeList<AppCBDynamic>
            {
                Items = new List<AppCBDynamic>
                {
                    new AppCBDynamic { Id = Guid.NewGuid(), Code = "1234567890123" },
                    new AppCBDynamic { Id = Guid.NewGuid(), Code = "9876543210987" }
                },
                PageIndex = pageIndex,
                TotalCount = 2
            };
            _mockBarcodeRepository.Setup(repo => repo.GetCBDynamicProducts(userGuid, pageIndex, pageSize))
                .ReturnsAsync(expectedProducts);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.GetCBDynamicProducts(userGuid, pageIndex, pageSize);

            // Assert
            Assert.That(result, Is.EqualTo(expectedProducts));
        }

        /// <summary>
        /// Tests the GetAllCBStatic method to verify it returns all static barcodes for a user.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetAllCBStatic method returns a list of all static barcodes when provided with a valid userGuid.
        /// </remarks>
        [Test()]
        public void GetAllCBStaticTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var expectedProducts = new List<AppCBStatic>
            {
                new AppCBStatic { Id = Guid.NewGuid(), Code = "1234567890123" },
                new AppCBStatic { Id = Guid.NewGuid(), Code = "9876543210987" }
            };
            _mockBarcodeRepository.Setup(repo => repo.GetAllCBStatic(userGuid))
                .Returns(expectedProducts);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = barcodeRepository.GetAllCBStatic(userGuid);

            // Assert
            Assert.That(result, Is.EqualTo(expectedProducts));
        }

        /// <summary>
        /// Tests the GetAllCBDynamic method to verify it returns all dynamic barcodes for a user.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetAllCBDynamic method returns a list of all dynamic barcodes when provided with a valid userGuid.
        /// </remarks>
        [Test()]
        public void GetAllCBDynamicTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var expectedProducts = new List<AppCBDynamic>
            {
                new AppCBDynamic { Id = Guid.NewGuid(), Code = "1234567890123" },
                new AppCBDynamic { Id = Guid.NewGuid(), Code = "9876543210987" }
            };
            _mockBarcodeRepository.Setup(repo => repo.GetAllCBDynamic(userGuid))
                .Returns(expectedProducts);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = barcodeRepository.GetAllCBDynamic(userGuid);

            // Assert
            Assert.That(result, Is.EqualTo(expectedProducts));
        }

        /// <summary>
        /// Tests the GetCBStaticById method to verify it returns the correct static barcode by its ID.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBStaticById method returns the correct static barcode when provided with a valid userGuid and barcode ID.
        /// </remarks>
        [Test()]
        public async Task GetCBStaticByIdTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            var expectedBarcode = new AppCBStatic { Id = barcodeId, Code = "1234567890123" };
            _mockBarcodeRepository.Setup(repo => repo.GetCBStaticById(userGuid, barcodeId))
                .ReturnsAsync(expectedBarcode);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.GetCBStaticById(userGuid, barcodeId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBarcode));
        }

        /// <summary>
        /// Tests the GetCBDynamicById method to verify it returns the correct dynamic barcode by its ID.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBDynamicById method returns the correct dynamic barcode when provided with a valid userGuid and barcode ID.
        /// </remarks>
        [Test()]
        public async Task GetCBDynamicByIdTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            var expectedBarcode = new AppCBDynamic { Id = barcodeId, Code = "1234567890123" };
            _mockBarcodeRepository.Setup(repo => repo.GetCBDynamicById(userGuid, barcodeId))
                .ReturnsAsync(expectedBarcode);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.GetCBDynamicById(userGuid, barcodeId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBarcode));
        }

        /// <summary>
        /// Tests the AddCBStatic method to verify it successfully adds a new static barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the AddCBStatic method returns the ID of the newly added static barcode when provided with a valid AppCBStatic object.
        /// </remarks>
        [Test()]
        public async Task AddCBStaticTest_Success()
        {
            // Arrange
            var newBarcode = new AppCBStatic { Code = "1234567890123" };
            var expectedBarcodeId = Guid.NewGuid().ToString();
            _mockBarcodeRepository.Setup(repo => repo.AddCBStatic(newBarcode))
                .ReturnsAsync(expectedBarcodeId);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.AddCBStatic(newBarcode);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBarcodeId));
        }

        /// <summary>
        /// Tests the AddCBDynamic method to verify it successfully adds a new dynamic barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the AddCBDynamic method returns the ID of the newly added dynamic barcode when provided with a valid AppCBDynamic object.
        /// </remarks>
        [Test()]
        public async Task AddCBDynamicTest_Success()
        {
            // Arrange
            var newBarcode = new AppCBDynamic { Code = "1234567890123" };
            var expectedBarcodeId = Guid.NewGuid().ToString();
            _mockBarcodeRepository.Setup(repo => repo.AddCBDynamic(newBarcode))
                .ReturnsAsync(expectedBarcodeId);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.AddCBDynamic(newBarcode);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBarcodeId));
        }

        /// <summary>
        /// Tests the UpdateCBStatic method to verify it successfully updates a static barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the UpdateCBStatic method returns true when provided with valid parameters.
        /// </remarks>
        [Test()]
        public async Task UpdateCBStaticTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            var modBarcode = new AppCBStatic { Id = barcodeId, Code = "1234567890123" };
            _mockBarcodeRepository.Setup(repo => repo.UpdateCBStatic(userGuid, barcodeId, modBarcode))
                .ReturnsAsync(true);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.UpdateCBStatic(userGuid, barcodeId, modBarcode);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the UpdateCBDynamic method to verify it successfully updates a dynamic barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the UpdateCBDynamic method returns true when provided with valid parameters.
        /// </remarks>
        [Test()]
        public async Task UpdateCBDynamicTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            var modBarcode = new AppCBDynamic { Id = barcodeId, Code = "1234567890123" };
            _mockBarcodeRepository.Setup(repo => repo.UpdateCBDynamic(userGuid, barcodeId, modBarcode))
                .ReturnsAsync(true);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.UpdateCBDynamic(userGuid, barcodeId, modBarcode);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the DeleteCBStatic method to verify it successfully deletes a static barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the DeleteCBStatic method returns true when provided with valid parameters.
        /// </remarks>
        [Test()]
        public async Task DeleteCBStaticTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcode = new AppCBStatic { Id = Guid.NewGuid(), Code = "1234567890123" };
            _mockBarcodeRepository.Setup(repo => repo.DeleteCBStatic(userGuid, barcode))
                .ReturnsAsync(true);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.DeleteCBStatic(userGuid, barcode);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the DeleteCBDynamic method to verify it successfully deletes a dynamic barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the DeleteCBDynamic method returns true when provided with valid parameters.
        /// </remarks>
        [Test()]
        public async Task DeleteCBDynamicTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcode = new AppCBDynamic { Id = Guid.NewGuid(), Code = "1234567890123" };
            _mockBarcodeRepository.Setup(repo => repo.DeleteCBDynamic(userGuid, barcode))
                .ReturnsAsync(true);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.DeleteCBDynamic(userGuid, barcode);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the DeleteCBStaticById method to verify it successfully deletes a static barcode by its ID.
        /// </summary>
        /// <remarks>
        /// This test checks if the DeleteCBStaticById method returns true when provided with a valid userGuid and barcode ID.
        /// </remarks>
        [Test()]
        public async Task DeleteCBStaticByIdTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            _mockBarcodeRepository.Setup(repo => repo.DeleteCBStaticById(userGuid, barcodeId))
                .ReturnsAsync(true);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.DeleteCBStaticById(userGuid, barcodeId);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the DeleteCBDynamicById method to verify it successfully deletes a dynamic barcode by its ID.
        /// </summary>
        /// <remarks>
        /// This test checks if the DeleteCBDynamicById method returns true when provided with a valid userGuid and barcode ID.
        /// </remarks>
        [Test()]
        public async Task DeleteCBDynamicByIdTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            _mockBarcodeRepository.Setup(repo => repo.DeleteCBDynamicById(userGuid, barcodeId))
                .ReturnsAsync(true);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.DeleteCBDynamicById(userGuid, barcodeId);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the GetCBStaticByCode method to verify it returns the correct static barcode by its code.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBStaticByCode method returns the correct static barcode when provided with a valid userGuid and code.
        /// </remarks>
        [Test()]
        public async Task GetCBStaticByCodeTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var code = "1234567890123";
            var expectedBarcode = new AppCBStatic { Id = Guid.NewGuid(), Code = code };
            _mockBarcodeRepository.Setup(repo => repo.GetCBStaticByCode(userGuid, code))
                .ReturnsAsync(expectedBarcode);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.GetCBStaticByCode(userGuid, code);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBarcode));
        }

        /// <summary>
        /// Tests the GetCBDynamicByCode method to verify it returns the correct dynamic barcode by its code.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBDynamicByCode method returns the correct dynamic barcode when provided with a valid userGuid and code.
        /// </remarks>
        [Test()]
        public async Task GetCBDynamicByCodeTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var code = "1234567890123";
            var expectedBarcode = new AppCBDynamic { Id = Guid.NewGuid(), Code = code };
            _mockBarcodeRepository.Setup(repo => repo.GetCBDynamicByCode(userGuid, code))
                .ReturnsAsync(expectedBarcode);

            var barcodeRepository = _mockBarcodeRepository.Object;

            // Act
            var result = await barcodeRepository.GetCBDynamicByCode(userGuid, code);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBarcode));
        }
    }
}
