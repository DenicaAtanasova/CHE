namespace CHE.Services.Data.Tests.Mocks
{
    using Moq;

    using static MockConstants;

    public class AddressesServiceMock
    {
        public static IAddressesService Instance
        {
            get
            {
                var addressesService = new Mock<IAddressesService>();
                addressesService.Setup(x => x.GetAddressIdAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(AddressMock.Id);

                return addressesService.Object;
            }
        }
    }
}