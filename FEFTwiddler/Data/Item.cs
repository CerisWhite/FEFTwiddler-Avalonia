namespace FEFTwiddler.Data
{
    public class Item
    {
        public Enums.Item ItemID { get; set; }
        public string DisplayName { get; set; }
        public Enums.ItemType Type { get; set; }
        public Enums.ItemSubType SubType { get; set; }
        public Enums.WeaponRank WeaponRank { get; set; }

        /// <summary>
        /// The maximum number of uses for this item. 0 = no maximum
        /// </summary>
        public byte MaximumUses { get; set; }

        /// <summary>
        /// Whether the item is normally only seen in the hands of an enemy or NPC
        /// </summary>
        public bool IsNpcOnly { get; set; }

        /// <summary>
        /// Whether the item disappears when leaving a map (example: Chest Key)
        /// </summary>
        public bool IsMapOnly { get; set; }

        /// <summary>
        /// Returns the avares:// URI for the item type icon.
        /// </summary>
        public string GetIconPath()
        {
            var imageName = SubType switch
            {
                Enums.ItemSubType.Sword => "ItemSubType_Sword",
                Enums.ItemSubType.Katana => "ItemSubType_Katana",
                Enums.ItemSubType.Lance => "ItemSubType_Lance",
                Enums.ItemSubType.Naginata => "ItemSubType_Naginata",
                Enums.ItemSubType.Axe => "ItemSubType_Axe",
                Enums.ItemSubType.Club => "ItemSubType_Club",
                Enums.ItemSubType.Dagger => "ItemSubType_Dagger",
                Enums.ItemSubType.Shuriken => "ItemSubType_Shuriken",
                Enums.ItemSubType.Bow => "ItemSubType_Bow",
                Enums.ItemSubType.Yumi => "ItemSubType_Yumi",
                Enums.ItemSubType.Tome => "ItemSubType_Tome",
                Enums.ItemSubType.Scroll => "ItemSubType_Scroll",
                Enums.ItemSubType.Staff => "ItemSubType_Staff",
                Enums.ItemSubType.Rod => "ItemSubType_Rod",
                Enums.ItemSubType.Dragonstone => "ItemSubType_Dragonstone",
                Enums.ItemSubType.Beaststone => "ItemSubType_Beaststone",
                Enums.ItemSubType.HealingItem => "ItemSubType_HealingItem",
                Enums.ItemSubType.StatTonic or Enums.ItemSubType.StatBooster => "ItemSubType_StatBooster",
                Enums.ItemSubType.SpecialConsumable => "ItemSubType_SpecialConsumable",
                Enums.ItemSubType.ClassChanger => "ItemSubType_ClassChanger",
                Enums.ItemSubType.SkillScroll => "ItemSubType_SkillScroll",
                Enums.ItemSubType.Emblem => "ItemSubType_Emblem",
                Enums.ItemSubType.Breath => "ItemSubType_Breath",
                Enums.ItemSubType.Fist => "ItemSubType_Fist",
                Enums.ItemSubType.Rock => "ItemSubType_Rock",
                Enums.ItemSubType.Saw => "ItemSubType_Saw",
                Enums.ItemSubType.GoldBar => "ItemSubType_GoldBar",
                Enums.ItemSubType.Gold => "ItemSubType_Gold",
                Enums.ItemSubType.Key => "ItemSubType_Key",
                Enums.ItemSubType.Obstacle => "ItemSubType_Obstacle",
                _ => "ItemSubType_Unknown"
            };

            return $"avares://FEFTwiddler/Resources/Images/ItemIcons/{imageName}.png";
        }
    }
}
